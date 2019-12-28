// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="bank-list.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector } from '@angular/core';
import { SFUISchema, SFSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { ACLService } from '@delon/acl';

@Component({
  selector: 'app-bank-list',
  templateUrl: './bank-list.component.html',
  styles: []
})
export class BankListComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.injector=injector;
    this.moduleName = 'bankList';
  }

  
  injector:Injector;

  get Acl():ACLService{
    return this.injector.get(ACLService);
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.CashMoney.BankList.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.CashMoney.BankList.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '用户ID', index: 'UserId', type: 'number',editable:true,filterable: true,acl:"Root.Admin.CashMoney.BankList.ReadMore"},
      { title: '开户姓名', index: 'Name', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '银行账号', index: 'Account', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '银行名称', index: 'BankName', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '开户银行', index: 'BankType', sort: true, editable: true, filterable: true, ftype: 'string' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新时间', index: 'LastUpdatedTime', type: 'date' },
    ];
    if(this.Acl.can("Root.Admin.CashMoney.BankList.ReadMore")){
      columns.push(
        { title: '创建者', index: 'CreatorId', type: 'number' },
        { title: '更新者', index: 'LastUpdaterId', type: 'number' },
        { title: '更新时间', index: 'LastUpdatedTime', type: 'date' },
      );
    }
    return columns;
  }

  protected GetSFSchema(): SFSchema {

    let schema: SFSchema = {
      properties: this.ColumnsToSchemas(this.columns),
      required:['Name','Account','BankName']
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Name:{grid:{span:24}},
      $Account:{grid:{span:24}},
      $BankName:{grid:{span:24}},
      $BankType:{grid:{span:24}},
      $UserId: {acl:"Root.Admin.CashMoney.BankList.ReadMore"},
    };
    return ui;
  }
}

