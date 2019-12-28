// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="merchant-extra.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { SFUISchema, SFSchema, SFComponent } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { NzModalComponent, NzMessageService } from 'ng-zorro-antd';
import { AjaxResult } from '@shared/osharp/osharp.model';
import { ACLService } from '@delon/acl';

@Component({
  selector: 'app-merchant-extra',
  templateUrl: './merchant-extra.component.html',
  styles: [],
})
export class MerchantExtraComponent extends STComponentBase implements OnInit {
  constructor(injector: Injector, protected msg: NzMessageService) {
    super(injector);
    this.moduleName = 'merchantExtra';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作',
        fixed: 'left',
        width: 65,
        buttons: [
          {
            text: '操作',
            children: [
              {
                text: '编辑',
                icon: 'edit',
                acl: 'Root.Admin.Merchant.MerchantExtra.Update',
                iif: row => row.Updatable,
                click: row => this.edit(row),
              },
            ],
          },
        ],
      },
      {
        title: '用户ID',
        index: 'UserId',
        type: 'number',
        sort: true,
        readOnly: true,
        editable: false,
        filterable: true,
      },
      {
        title: '上级ID',
        index: 'PUserId',
        type: 'number',
        sort: true,
        editable: true,
        filterable: true,
        ui: { acl: 'Root.Admin.Merchant.MerchantExtra.UpdatePUserId' },
        render: 'puserid',
      },
      { title: '更多信息', index: 'Extra', ftype: 'string', render: 'extra' },
      {
        title: '接入秘钥',
        index: 'key',
        editable: false,
        ftype: 'string',
        acl: 'Root.Admin.Merchant.MerchantExtra.ReadKey',
        ui: { acl: 'Root.Admin.Merchant.MerchantExtra.UpdateKey' },
        render: 'key',
      },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
    };
    return ui;
  }

  public showextrajson: any;
  public showextraui = {};
  @ViewChild('extrashowsf', { static: false }) extrashowsf: SFComponent;
  @ViewChild('extrashowmodal', { static: false }) extrashowmodal: NzModalComponent;
   showextra(item, index) {
    if (item.Extra === null) {
      this.msg.info('暂无更多信息');
      return;
    }
    let ownProps = Object.keys(item.Extra);
    let i = ownProps.length;
    let resArray = new Array(i);
    while (i--) resArray[i] = `${ownProps[i]}:${item.Extra[ownProps[i]]}`;
    this.showextrajson = resArray;
    console.log(this.showextrajson);
    this.extrashowmodal.open();
    console.log(item);
  }

   closeextra() {
    this.extrashowmodal.close();
  }
   updatekeym(item) {
    let updatekeyurl = `api/admin/${this.moduleName}/UpdateKey`;
    this.http.post<AjaxResult>(updatekeyurl, item).subscribe(result => {
      this.osharp.ajaxResult(result, () => {
        this.st.reload();
        this.editModal.destroy();
      });
    });
  }
   cancelupdatekey(item){
    console.log(item);
  }
}
