// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="amounts-log.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector } from '@angular/core';
import { SFUISchema, SFSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';

@Component({
  selector: 'app-amounts-log',
  templateUrl: './amounts-log.component.html',
  styles: []
})
export class AmountsLogComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'amountsLog';
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
      { title: '用户ID', index: 'UserId', type: 'number', readOnly: true ,filterable: true,},
      { title: '记录ID', index: 'PayID', filterable: true, ftype: 'string' },
      { title: '影响类型', index: 'PayType', filterable: true, ftype: 'string' },
      { title: '影响后金额', index: 'AfterAmounts', type: 'number' },
      { title: '影响前金额', index: 'BeforeAmounts', type: 'number' },
      { title: '影响金额', index: 'Amounts', type: 'number' },
      { title: '备注说明', index: 'Remarks', ftype: 'string' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $PayID: { grid: { span: 24 } },
      $PayType: { grid: { span: 24 } },
      $Remarks: { grid: { span: 24 } }
    };
    return ui;
  }
}

