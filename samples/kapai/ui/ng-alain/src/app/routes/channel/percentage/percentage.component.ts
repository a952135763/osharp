// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="percentage.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector, EventEmitter, Output, ViewChild, ElementRef } from '@angular/core';
import { SFUISchema, SFSchema, SFSchemaEnumType, SFSchemaType, SFComponent } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { ApiService } from '@shared/osharp/services/api.service';
import { of, from, fromEvent } from 'rxjs';

@Component({
  selector: 'app-percentage',
  templateUrl: './percentage.component.html',
  styles: [],
})
export class PercentageComponent extends STComponentBase implements OnInit {

  constructor(injector: Injector, protected api: ApiService) {
    super(injector);
    this.moduleName = 'percentage';
  }


  protected userid = 0;
  protected channelid: string = null;
  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },

      { title: '用户ID', index: 'UserId', type: 'number', filterable: true, editable: true },
      { title: '通道名称', index: 'Channel_Name', ftype: 'string', filterable: true },
      { title: '费率名称', index: 'Name', editable: true, filterable: true, ftype: 'string' },
      {
        title: '费率值',
        index: 'Value',
        sort: true,
        editable: true,
        type: 'number',
        minimum: 0,
        maximum: 1000,
        render: 'Value',
      },

      { title: '创建者', index: 'CreatorId', type: 'number' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新者', index: 'LastUpdaterId', type: 'number' },
      { title: '更新时间', index: 'LastUpdatedTime', type: 'date' },
    ];
    return columns;
  }

  protected GetSFSchema(): SFSchema {
    let sch = this.ColumnsToSchemas(this.columns);
    let c: { ChannelId: SFSchema } = { ChannelId: { type: 'string', title: '通道' } };
    Object.assign(c, sch);
    let schema: SFSchema = {
      properties: c,
      required: ['Name', 'Value', 'ChannelId', 'UserId'],
    };
    return schema;
  }

   EditVal(item: any) {
    console.log(item);
  }


  protected SelectName() {
    if (this.userid && this.channelid) {
      this.api.ReadPercentageName(this.userid, this.channelid).subscribe(r => {
        const arr = [];
        r.forEach(val => {
          arr.push({ label: val, value: val });
        });
        let myEvent = new CustomEvent('UserId_select', {
          detail: {
            Names: arr,
          },
        });
        window.dispatchEvent(myEvent);
      });
    }
  }

  close() {
    if (!this.editModal) return;
    this.editModal.destroy();
    this.userid = 0;
    this.channelid = null;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 }, order: ['Id', 'ChannelId', 'UserId', '*'] },
      $Id: { grid: { span: 24 } },
      $Name: {
        grid: { span: 24 },
        widget: 'select',
        asyncData: () => {
          return fromEvent(window, 'UserId_select').map((eve: CustomEvent) => {
            return eve.detail.Names;
          });
        },
      },
      $ChannelId: {
        widget: 'select',
        asyncData: () =>
          this.api.ReadChannels().map((v: any[]) => {
            const arr = [];
            v.forEach(value => {
              arr.push({ label: value.Name, value: value.Id });
            });
            return arr;
          }),
        change: (m: any) => {
          this.channelid = m;
          this.SelectName();
        },
      },
      $UserId: {
        widget: 'select',
        asyncData: () => this.api.ReadAllMerchant(),
        change: (m: any) => {
          this.userid = m;
          this.SelectName();
        },
      },
      $Value: {},
    };
    return ui;

  }
}
