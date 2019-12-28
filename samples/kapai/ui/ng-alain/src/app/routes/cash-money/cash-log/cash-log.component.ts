// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="cash-log.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector } from '@angular/core';
import { SFUISchema, SFSchema, SFSelectWidgetSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { ACLService } from '@delon/acl';
import { of } from 'rxjs';
import { MerchantService } from '@shared/osharp/services/merchant.service';
import { map } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd';
import { AjaxResultType } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-cash-log',
  templateUrl: './cash-log.component.html',
  styles: []
})

export class CashLogComponent extends STComponentBase implements OnInit {




  injector:Injector;

  get Acl():ACLService{
    return this.injector.get(ACLService);
  }

  constructor(injector: Injector,public merchantService:MerchantService,protected msg: NzMessageService,) {
    super(injector);
    this.injector=injector;
    this.moduleName = 'cashLog';
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
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '用户ID', index: 'UserId', type: 'number', editable: true, filterable: true, acl:"Root.Admin.CashMoney.CashLog.ReadMore"},
      { title: '处理用户', index: 'ProUserId', type: 'number',filterable: true, },
      { title: '名称', index: 'Name', sort: true, filterable: true, ftype: 'string'},
      { title: '开户银行', index: 'BankType', sort: true, filterable: true, ftype: 'string' },
      { title: '收款账号', index: 'Account', sort: true, filterable: true, ftype: 'string' },
      { title: '收款银行', index: 'BankName', sort: true, filterable: true, ftype: 'string' },
      { title: '备注', index: 'Remarks', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新时间', index: 'LastUpdatedTime', sort: true, filterable: true,type: 'date' },
      { title: '处理状态', index: 'Status', sort: true, filterable: true, type: 'tag', },
    ];
    if(this.Acl.can("Root.Admin.CashMoney.CashLog.ReadMore")){
      columns.push(
      { title: '任务Id', index: 'JobId', sort: true, filterable: true, ftype: 'string' },
      { title: '创建者', index: 'CreatorId', type: 'number' },
      { title: '更新者', index: 'LastUpdaterId', type: 'number' }
      );
    }
    return columns;
  }


  protected GetSFSchema(): SFSchema {
    let tos = this.ColumnsToSchemas(this.columns);
    tos.Point = {type:'number',title:'金额'};
    tos.Bank = {type:'string',title:'目标号'};
  
    let schema: SFSchema = {
      properties: tos,
      required:['Point','Bank'],
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    // TODO: Point 需要默认显示当前余额...
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Remarks: { grid: { span: 24 } },
      $UserId:{ acl:"Root.Admin.CashMoney.CashLog.ReadMore"},
      $Bank:{ grid: { span: 24 },widget: 'select',asyncData:()=>this.merchantService.ReadBank()},
      $Point:{ grid: { span: 24 }}
    };
    return ui;
  }
 Cash(o:any){
    this.merchantService.Cash(o).subscribe((res)=>{
      // 成功
      if(res.Type === AjaxResultType.Success) {
        this.msg.success("申请成功");
      }else{
        // 申请错误
      }
      
    },(error)=>{
      // 出错
      this.msg.error("申请出错:"+error);
    });
  }


}

