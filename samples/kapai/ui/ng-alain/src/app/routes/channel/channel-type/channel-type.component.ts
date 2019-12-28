// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="channel-type.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { SFUISchema, SFSchema } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { NzModalComponent, NzMessageService } from 'ng-zorro-antd';
import { ApiService } from '@shared/osharp/services/api.service';
import { AdchanneltypeComponent } from '@shared/components/ad-channeltype/ad-channeltype.component';

@Component({
  selector: 'app-channel-type',
  templateUrl: './channel-type.component.html',
  styles: [],
})
export class ChannelTypeComponent extends STComponentBase implements OnInit {
  @ViewChild('channeltype', { static: false }) channeltype: AdchanneltypeComponent;

  constructor(injector: Injector, protected api: ApiService, protected msg: NzMessageService) {
    super(injector);
    this.moduleName = 'channelType';
  }

  ngOnInit() {
    super.InitBase();
  }

  protected GetSTColumns(): OsharpSTColumn[] {
    let columns: OsharpSTColumn[] = [
      { title: '编号', index: 'Id', sort: true, readOnly: true, editable: true, filterable: true, ftype: 'string' },
      { title: '通道名称', index: 'Channel_Name', ftype: 'string', filterable: true },
      { title: '参数模板', index: 'ChannelJson', ftype: 'string', render: 'ChannelJson' },
      { title: '创建时间', index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新者', index: 'LastUpdaterId', type: 'number' },
      { title: '更新时间', index: 'LastUpdatedTime', type: 'date' },
    ];
    return columns;
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
    };
    return ui;
  }

   opennewmodel(){
    this.channeltype.opennewmodel();
  }
   ChannelJsonShow(a){
    this.channeltype.ChannelJsonnShow(a);
  }

  success(){
    this.st.reload();
  }

}


