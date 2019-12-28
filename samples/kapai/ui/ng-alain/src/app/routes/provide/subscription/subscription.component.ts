// ------------------------------------------------------------------------------
//
//  <copyright file="subscription.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector, Optional, ViewChild } from '@angular/core';
import { SFSchema, SFComponent } from '@delon/form';
import * as MessagePack from '@microsoft/signalr-protocol-msgpack';
import * as signalR from '@microsoft/signalr';
import { IdentityService } from '@shared/osharp/services/identity.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { STComponent } from '@delon/abc';
import {
  DataEvent,
  DragEndEvent,
  DragOverEvent,
  DragStartEvent,
  NavigateEvent,
  SortableComponent,
} from '@progress/kendo-angular-sortable';

@Component({
  selector: 'app-subscription',
  templateUrl: './subscription.component.html',
  styles: [],
})
export class SubscriptionComponent implements OnInit {
  connection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Error)
    .withUrl('http://localhost:7003/hub/prohub', { accessTokenFactory: () => this.identity.getAccessToken().token })
    .withHubProtocol(new MessagePack.MessagePackHubProtocol())
    .build();

  isSpinning = true;
  submitdisabled = true;
  resetdisabled = true;
  tipStr = '连接服务器中..';
  items: any[] = [];
  isSpecial = true;
  ArticleEntEnum: any[];
  restartHub = true;
  itemloading = false;
  itemtipStr = '已锁定..';
  modalshow = false;
  orderComplete: any = {};
  ordertip = '';
  // 防止重复加入
  datahash: Map<string, any> = new Map();

  @ViewChild('sf', { static: false }) sf: SFComponent;
  @ViewChild('st', { static: false }) st: STComponent;
  @ViewChild('sortable', { static: false }) sortable: SortableComponent;
  // 表单选择器
  schema: SFSchema = {
    properties: {
      ArticleAssort: {
        type: 'string',
        title: '选择分类',
        default: null, // 默认值
        ui: {
          widget: 'select', // 单选框渲染
          showSearch: true, // 可输入名字搜索
          placeholder: '选择分类', // 默认提示消息
          width: 200,
          change: (ng: any) => {
            this.ArticleAssortchange(ng);
          },
        },
      },
      ArticleEnt: {
        type: 'string',
        title: '选择账号',
        default: null, // 默认值
        ui: {
          widget: 'select', // 单选框渲染
          showSearch: true, // 可输入名字搜索
          placeholder: '选择账号', // 默认提示消息
          width: 300,
        },
      },
    },
  };

  // 方法区

  constructor(
    @Optional() protected injector: Injector,
    protected msg: NzMessageService,
    protected modalService: NzModalService,
  ) {}
  private get identity(): IdentityService {
    return this.injector.get(IdentityService);
  }

  ngOnInit(): void {
    // 监听认证成功数据,并获取账号分类列表
    this.connection.on('Open', (list, child) => {
      this.ArticleEntEnum = child;
      this.schema.properties.ArticleAssort.enum = list;
      this.sf.refreshSchema();
      this.isSpinning = false;
      this.submitdisabled = false;
      this.resetdisabled = true;
    });

    // 监听开始抢单事件!
    this.connection.on('GrabReturn', (msg: string) => {
      console.log(msg);
      if (msg === 'Ok') {
        this.msg.success('开始监听订单..');
        this.submitdisabled = true;
        this.resetdisabled = false;
      } else {
        this.submitdisabled = false;
        this.resetdisabled = true;
        this.msg.error(msg);
      }
    });

    // 停止事件
    this.connection.on('StopGrab', (msg: string) => {
      if (msg === 'Ok') {
        // 移除订单列表!

        this.submitdisabled = false;
        this.resetdisabled = true;
      } else {
        this.msg.error(msg);
      }
    });

    // 定义关闭事件
    this.connection.onclose(e => {
      if (!this.isSpinning) this.isSpinning = true;
      this.clearsortable();
      // 是否重连....
      this.Hubrestart();
    });

    // 监听服务器关闭事件
    this.connection.on('HubClose', (msg: string) => {
      this.msg.error(msg);
      this.restartHub = false;
      this.connection.stop();
    });

    // 业务监听信息
    // 监听订单消息 增加或删除
    this.connection.on('Order', (order: any) => {
      // {orderid,createdTime,amount,action}
      if (this.isSpinning || !this.submitdisabled || this.resetdisabled) {
        console.log('丢弃消息', order);
        return;
      }
      switch (order.action) {
        case 0: // 清除此订单
          this.ItemRemove(order);
          break;
        case 1: // 增加订单
          this.ItemAdd(order);
          break;
        case 2:
          return this.ItemClear();
      }
      this.Showorder();
    });

    // 监听抢订单返回信息 抢成功 就锁页面,等待此单完成,否则为抢单失败 失败从列表删除
    this.connection.on('OrderReturn', (order: any) => {
      // 抢订单返回信息 抢订单成功 显示订单待确认页面 抢单失败解锁页面 删除订单继续监听
      switch (order.action) {
        case 0:
          // 抢单失败!
          this.msg.warning('Sorry!您手慢了!');
          this.itemloading = false;
          this.ItemRemove(order);
          break;
        case 1:
          // 显示出此订单详细状态,待用户确认或者等待超时!
          this.orderComplete = order;
          this.ordertip = `订单号:${order.orderid}`;
          this.modalshow = true;
          break;
        case 2:
          // 账号状态不正常!
          this.msg.warning('当前收款号不正常!');
          this.itemloading = false;
          this.ItemRemove(order);
          break;
      }
      this.Showorder();
    });

    this.connection.on('ArrivalReturn', (order: any) => {
      if (this.modalshow === true) {
        if (order.action === 0) this.msg.warning('确认失败!请联系客服');
        if (order.action === 1) {
          this.msg.success(`${order.orderid}确认成功`);
          this.modalshow = false;
          this.orderComplete = {};
          this.ordertip = '';
          this.itemloading = false;
          this.ItemRemove(order);
          this.Showorder();
        }
      }
    });

    this.Hubstart();
  }
  // 选择分类 加载分类下的账号
  ArticleAssortchange(ng): void {
    if (ng) {
      let arr = [];
      this.ArticleEntEnum.some(element => {
        if (element.pid === ng) {
          arr.push(element);
        }
      });
      this.schema.properties.ArticleEnt.enum = arr;
      this.schema.properties.ArticleAssort.default = ng;
      setTimeout(() => {
        this.sf.refreshSchema();
      }, 50);
    }
  }

  Showorder(): void {
    if (this.items.length > 0) {
      if (this.isSpecial) this.isSpecial = false;
    } else {
      if (!this.isSpecial) this.isSpecial = true;
    }
  }

  clearsortable(): void {
    this.sortable.dragIndex = -1;
    this.sortable.dragOverIndex = -1;
    this.sortable._localData.length = 0;
    this.sortable.data.length = 0;
    this.items.length = 0;
    this.sortable.updateCacheIndices();
    this.Showorder();
  }

  // 清除订单 和 防重复哈希表
  ItemClear(): void {
    this.datahash.clear();
    this.clearsortable();
  }

  // 删除指定 订单信息
  ItemRemove(order: any): void {
    if (this.datahash.has(order.orderid)) {
      this.datahash.delete(order.orderid);
      let i = this.items.findIndex(o => o.orderid === order.orderid);
      if (i > -1) this.sortable.removeDataItem(i);
    }
  }

  // 添加订单信息
  ItemAdd(order: any, index: number = 0): void {
    if (!this.datahash.has(order.orderid)) {
      this.datahash.set(order.orderid, order);
      this.sortable.addDataItem(order, index);
    }
  }

  // 重连
  Hubrestart() {
    if (this.restartHub) {
      setTimeout(() => {
        this.Hubstart().catch(() => this.Hubrestart());
      }, 5000);
    }
  }

  // 连接开始
  Hubstart(): Promise<void> {
    return this.connection.start().then(() => {
      this.tipStr = '等待信息中..';
    });
  }

  // 账号和账号分类确认 开始监听订单
  submit(value: any) {
    if (value.ArticleAssort != null && value.ArticleEnt !== null) {
      if (this.resetdisabled && this.submitdisabled === false) {
        this.submitdisabled = true;
        this.connection
          .invoke('News', new NowData(Cmd.StartGrab,{ ArticleAssortId: value.ArticleAssort, ArticleId: value.ArticleEnt }))
          .catch(() => {
            // 发生错误
            this.submitdisabled = false;
          });
      } else {
        this.msg.error('状态不正常!请刷新页面');
      }
      // 开始监听抢单信息
    } else {
      this.msg.error('分类或账号为空');
    }
    // this.msg.success();
  }

  // 取消监听订单
  rest(value: any) {
    if (!this.resetdisabled && this.submitdisabled) {
      // 发送停止抢单
      this.connection
        .invoke('News',new NowData(Cmd.StopGrab,null))
        .then(() => {
          this.clearsortable();
        })
        .catch(() => {
          // 发生错误
          this.resetdisabled = false;
        });
    } else {
      this.msg.error('状态不正常!请刷新页面');
    }
  }

  // 抢一个订单!
  gorder(e: any) {
    // 发送抢单信息 ! 同时页面禁止再抢其他订单
    if (e && e.orderid) {
      this.modalService.confirm({
        nzTitle: `<i>确认本次操作!</i>`,
        nzContent: `<b>对:${e.orderid},执行抢单动作!</b>`,
        nzOnOk: () => {
          this.itemloading = true;
          this.connection.invoke('News', new NowData(Cmd.GrabOrder,{ OrderId: e.orderid, Time: Date.now() })).catch(error => {
            console.log('抢订单失败', error);
            this.itemloading = false;
          });
        },
      });
    }
  }
  ordercomplete(e: any) {
    if (e && e.orderid) {
      this.connection.invoke('News', new NowData(Cmd.Arrival,{ OrderId: e.orderid, Time: Date.now()})).catch(error => {
        this.msg.error(error);
      });
    }
  }

  // 拷贝成功消息
  cbsuccess(str: string) {
    this.msg.success(str + ';复制成功!');
  }

  // 数据被添加
  public onDataAdd(e: DataEvent): void {
    console.log('dataAdd', e.dataItem);
  }

  // 数据被移动!
  public onDataMove(e: DataEvent): void {
    console.log('dataMove', e.dataItem);
  }

  // 数据被删除
  public onDataRemove(e: DataEvent): void {
    console.log('dataRemove', e.dataItem);
  }

  public onDragEnd(e: DragEndEvent): void {
    e.preventDefault();
  }

  public onDragOver(e: DragOverEvent): void {
    console.log(e);
  }

  public onDragStart(e: DragStartEvent): void {
    e.preventDefault();
  }

  public onNavigate(e: NavigateEvent): void {
    e.preventDefault();
  }
}
export enum Cmd {
  Open,
  Close,
  StartGrab,
  StopGrab,
  GrabOrder,
  Arrival,
}
export class NowData{
  Cmd:string;
  Data:any;
  constructor(c:Cmd,d:any){
    this.Cmd = c.toString();
    this.Data = d;
  }
}
