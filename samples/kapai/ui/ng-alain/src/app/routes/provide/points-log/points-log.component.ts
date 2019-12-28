// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="points-log.module.ts">
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
  selector: 'app-points-log',
  templateUrl: './points-log.component.html',
  styles: []
})
export class PointsLogComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'pointsLog';
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
      { title: '用户Id', index: 'UserId', type: 'number' },
      { title: '动作Id', index: 'PayId', filterable: true, ftype: 'string' },
      { title: '影响类型', index: 'PayType', filterable: true, ftype: 'string' },
      { title: '影响前积分', index: 'BeforePoint', type: 'number' },
      { title: '影响后积分', index: 'AfterPoint', type: 'number' },
      { title: '影响积分', index: 'Point', type: 'number' },
      { title: '备注', index: 'Remarks', ftype: 'string' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let schema: SFSchema = {
      properties: this.ColumnsToSchemas(this.columns),
      required: ['PayId']
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $PayId: { grid: { span: 24 } },
      $PayType: { grid: { span: 24 } },
      $Remarks: { grid: { span: 24 } }
    };
    return ui;
  }
}

