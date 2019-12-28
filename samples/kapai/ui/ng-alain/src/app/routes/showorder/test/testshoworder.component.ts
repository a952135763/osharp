import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { _HttpClient, ModalHelper } from '@delon/theme';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { AjaxResult, AjaxResultType } from '@shared/osharp/osharp.model';
import { CountdownEvent } from 'ngx-countdown';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { retryWhen, map, catchError, delay } from 'rxjs/operators';
import { interval } from 'rxjs';

@Component({
  selector: 'app-testshoworder',
  templateUrl: './testshoworder.component.html',
  styles: [
    `
      nz-header {
        background-color: transparent;
        padding: 0;
      }
    `,
  ],
})
export class TestshoworderComponent implements OnInit {
  loading = true;
  orderid: string;
  qrurl: string;
  money = 0;
  target = 0;
  overnum = 0;
  tipstr = '请稍等片刻...';
  ClientId: string;
  latitude = 0;
  longitude = 0;
  constructor(
    private route: ActivatedRoute, 
    private http: _HttpClient, 
    private msgSrv: NzMessageService,
    private mod:NzModalService,
    ) {
    // 尝试获取定位
    this.getLocation();
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.ClientId = localStorage.getItem('ClientId');
      // 获取订单号
      this.orderid = this.route.snapshot.params.orderid;
      // 激活订单
      this.http
        .post('api/OpenApi/PortionOrder', {
          clientid: this.ClientId,
          id: this.orderid,
          latitude: this.latitude,
          longitude: this.longitude,
        })
        .subscribe((res: AjaxResult) => {
          console.log(res);
          if (res.Type === AjaxResultType.Error) {
            // 发生错误
            this.mod.warning({nzContent:res.Content});
          } else {
            // 轮训订单信息
            this.ClientId = res.Data.ClientId;
            localStorage.setItem('ClientId', this.ClientId);
            this.query();
          }
        });
    }, 1000 * 2);
  }

  /** 开始轮训订单信息 */
  protected query(recount = 0) {
    // 获取订单信息!

    this.http
      .post<AjaxResult>('api/OpenApi/ReadOrderInfo', { clientid: this.ClientId, id: this.orderid, pops: ['payurl'] })
      .pipe(
        map(r => {
          if (r && r.Type === AjaxResultType.Success) {
            return r;
          } else {
            throw new Error('未分配');
          }
        }),
      )
      .subscribe(
        r => {
          this.showorder(r.Data);
        },
        r => {
          ++recount;
          this.tipstr = `处理中...${recount}`;
          setTimeout(() => {
            this.query(recount);
          }, 1000 * 10);
        },
      );
  }

  protected getLocation() {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        position => {
          this.latitude = position.coords.latitude;
          this.longitude = position.coords.longitude;
        },
        error => {
          console.warn('无法获取定位');
          console.log(error);
        },
      );
    } else {
      console.log('该浏览器不支持。');
    }
  }

  protected showorder(order: any) {
    // 解析订单信息 并显示支付页面
    let outtime = new Date(order.OutTime);
    let second = (outtime.getTime() - new Date().getTime()) / 1000;
    if (second < 0) {
      this.msgSrv.error('订单已超时');
      return;
    }
    this.money = order.CreatedAmount / 100;
    this.qrurl = order.payurl;
    this.target = second;
    this.loading = false;
  }
  handleEvent(e: CountdownEvent) {
    console.log(e);
    
    if (e.action === 'done') {
      if (this.overnum === 0) {
        ++this.overnum;
        return;
      }
      
      // 倒计时已完
      console.log('倒计时已完毕,清空信息');
    }
  }
}
