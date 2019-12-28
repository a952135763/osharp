// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="article-assort.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved. 
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector } from '@angular/core';
import { SFUISchema, SFSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { ApiService } from '@shared/osharp/services/api.service';

@Component({
  selector: 'app-article-assort',
  templateUrl: './article-assort.component.html',
  styles: []
})
export class ArticleAssortComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector,protected api:ApiService) {
    super(injector);
    this.moduleName = 'articleAssort';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      {
        title: '操作', fixed: 'left', width: 65, buttons: [{
          text: '操作', children: [
            { text: '编辑', icon: 'edit', acl: 'Root.Admin.Provide.ArticleAssort.Update', iif: row => row.Updatable, click: row => this.edit(row) },
            { text: '删除', icon: 'delete', type: 'del', acl: 'Root.Admin.Provide.ArticleAssort.Delete', iif: row => row.Deletable, click: row => this.delete(row) },
          ]
        }]
      },
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '分类名称', index: 'Name', editable: true, filterable: true, ftype: 'string' },
      { title: '通道名称', index: 'Channel_Name', ftype: 'string' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
    ];
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let sch = this.ColumnsToSchemas(this.columns);
    sch.ChannelId = {
      type: 'string',
      title: '通道',
    };
    let schema: SFSchema = {
      properties: sch,
      required: ['Name','ChannelId']
    };
    return schema;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Name: { grid: { span: 24 } },
      $ChannelId:{ widget: 'select', asyncData:()=>this.api.ReadChannels().map((v:any[])=>{
        const arr = [];
        v.forEach((value)=>{
          console.log(value);
          
          arr.push({ label: value.Name, value: value.Id });
        });
        return arr;
      })},
    };
    return ui;
  }
}

