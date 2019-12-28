// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="channels.module.ts">
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
import { of } from 'rxjs';

@Component({
  selector: 'app-channels',
  templateUrl: './channels.component.html',
  styles: [],
})
export class ChannelsComponent extends STComponentBase implements OnInit {
  StatusTAG: STColumnTag = {
    0: { text: '未开通', color: 'gold' },
    1: { text: '已开通', color: 'green' },
  };

  constructor(injector: Injector) {
    super(injector);
    this.moduleName = 'channels';
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
                acl: 'Root.Admin.Channel.Channels.Update',
                iif: row => row.Updatable,
                click: row => this.edit(row),
              },
            ],
          },
        ],
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '通道代码', index: 'Code', editable: true, filterable: true, ftype: 'string' },
      { title: '通道名称', index: 'Name', editable: true, filterable: true, ftype: 'string' },
      { title: '最小限制', index: 'Minimum', editable: true, type: 'number', minimum: 0 },
      { title: '最大限制', index: 'Maxmum', editable: true, type: 'number', minimum: 0, maximum: 1000000 },
      { title: '状态', index: 'Status', editable: true, filterable: true, type: 'tag', tag: this.StatusTAG },
      { title: '创建者', index: 'CreatorId', type: 'number' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新者', index: 'LastUpdaterId', type: 'number' },
      { title: '更新时间', index: 'LastUpdatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let schema: SFSchema = {
      properties: this.ColumnsToSchemas(this.columns),
      required: ['Code', 'Name', 'Status', 'Minimum', 'Maxmum'],
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Code: { grid: { span: 24 }, },
      $Name: { grid: { span: 24 }, },
      $Minimum: { unit: '分', grid: { span: 12 }, },
      $Maxmum: { unit: '分', grid: { span: 12 }, },
      $Status: {
        widget: 'select',
        asyncData: () =>
          of(this.StatusTAG).map((v: STColumnTag) => {
            const arr = [];
            for (let key in v) {
              if (v.hasOwnProperty(key)) arr.push({ label: v[key].text, value: key });
            }
            return arr;
          }),
        grid: { span: 12 },
        
      },
    };
    return ui;
  }
}
