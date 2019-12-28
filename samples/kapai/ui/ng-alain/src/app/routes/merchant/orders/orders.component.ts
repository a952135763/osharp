// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="orders.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector } from '@angular/core';
import { SFUISchema, SFSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { STColumnTag } from '@delon/abc';
import { ACLService } from '@delon/acl';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styles: []
})
export class OrdersComponent extends STComponentBase implements OnInit {

  injector:Injector;

  OrderTAG: STColumnTag = {
    0: { text: '待处理', color: '' },
    1: { text: '处理中', color: 'lime' },
    2: { text: '已付款', color: 'gold' },
    3: { text: '已结算', color: 'orange' },
    4: { text: '已冻结', color: '#f50' },
    5: { text: '已超时', color: 'red' },
  };
  CallBackTAG: STColumnTag={
    0: { text: '待处理', color: '' },
    1: { text: '处理中', color: 'lime' },
    2: { text: '已付款', color: 'gold' },
  };

  get Acl():ACLService{
    return this.injector.get(ACLService);
  }


  constructor(injector: Injector) {
    super(injector);
    this.injector = injector;
    this.moduleName = 'orders';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
          ]
        }]
      },
      { title: '系统订单号', index: 'Id', readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '收款号ID', index: 'ArticleAssortId', sort: true, filterable: true, ftype: 'string' ,render: 'articleassort'},
      { title: '通道名称', index: 'Channel_Name', sort: true, filterable: true, ftype: 'string' ,render:"channel"},
      { title: '商户ID', index: 'UserId', sort: true, filterable: true, type: 'number',render:'userid'},
      { title: '商户订单号', index: 'Orderid', sort: true, filterable: true, ftype: 'string',render:"orderid" },
      { title: '创建金额', index: 'CreatedAmount', sort: true, type: 'number' ,render:"createdamount"},
      { title: '支付金额', index: 'PayAmount', sort: true, type: 'number' ,render:"payamount"},
      { title: '订单备注', index: 'Remark', ftype: 'string',render:"remark"},
      { title: '订单状态', index: 'Status', filterable: true, type: 'tag',tag: this.OrderTAG},
      { title: '回调状态', index: 'CallBack' ,filterable:true,type:'tag',tag: this.CallBackTAG },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新时间', index: 'LastUpdatedTime',sort: true, filterable: true, type: 'date'},
    ];
    // 是否可以读取更多数据
    if(this.Acl.can("Root.Admin.Merchant.Orders.ReadMore")){
      columns.push( { title: '客户唯一码', index: 'ClientId', filterable: true, ftype: 'string' ,acl:'Root.Admin.Merchant.Orders.ReadMore',ui:{acl:"Root.Admin.Merchant.Orders.ReadMore"}},
      { title: '客户ip', index: 'ClientIp', ftype: 'string' ,acl:'Root.Admin.Merchant.Orders.ReadMore',ui:{acl:"Root.Admin.Merchant.Orders.ReadMore"}},
      { title: '后台任务记录', index: 'JobId', ftype: 'string',acl:'Root.Admin.Merchant.Orders.ReadMore',ui:{acl:"Root.Admin.Merchant.Orders.ReadMore"} },)
    }
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let schema: SFSchema = {
      properties: this.ColumnsToSchemas(this.columns),
      required: ['Orderid', 'CreatedAmount', 'AsynUrl', 'Status']
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Orderid: { grid: { span: 24 } },
      $Remark: { grid: { span: 24 } },
      $ClientId: { grid: { span: 24 } },
      $ClientIp: { grid: { span: 24 } },
      $AsynUrl: { grid: { span: 24 } },
      $JobId: { grid: { span: 24 } }
    };
    return ui;
  }
}

