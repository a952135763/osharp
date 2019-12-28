// ------------------------------------------------------------------------------
//  <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//  </once-generated>
//
//  <copyright file="article-entities.module.ts">
//      KaPai©2019 Microsoft Corporation. All rights reserved.
//  </copyright>
//  <site></site>
//  <last-editor>KaPai</last-editor>
// -----------------------------------------------------------------------

import { Component, OnInit, Injector, Output, ViewChild, OnDestroy } from '@angular/core';
import { SFUISchema, SFSchema, SFComponent, SFSchemaType, SFSelectWidgetSchema, SFUISchemaItem } from '@delon/form';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { STComponentBase } from '@shared/osharp/components/st-component-base';
import { STData } from '@delon/abc';
import { NzModalComponent } from 'ng-zorro-antd';
import { of } from 'rxjs';
import { ApiService } from '@shared/osharp/services/api.service';
import { ProvideService } from '@shared/osharp/services/provide.service';
import { ACLService } from '@delon/acl';
import { ModalHelper } from '@delon/theme';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { AjaxResult } from '@shared/osharp/osharp.model';

@Component({
  selector: 'app-article-entities',
  templateUrl: './article-entities.component.html',
  styles: [],
})
export class ArticleEntitiesComponent extends STComponentBase implements OnInit {
  public showextrajson: any;
  public showextraui = {};
  isedit:boolean;
  @ViewChild('extrashowmodal', { static: false }) extrashowmodal: NzModalComponent;

  ReadMore: boolean;
  @ViewChild('sf', { static: false }) sf: SFComponent;
  region:any[] = [];

  _onReuseInit() {
    // 可以刷新页面
    console.log('_onReuseInit');
  }
  _onReuseDestroy() {
    // 路由被跳转
    console.log('_onReuseDestroy');
  }

  test:string;
  constructor(
    injector: Injector,
    protected api: ApiService,
    protected pro: ProvideService,
    protected acl: ACLService,
    protected nzmodel: ModalHelper,
    protected os: OsharpService,
  ) {
    super(injector);
    this.moduleName = 'articleEntities';
  }

  ngOnInit() {
    super.InitBase();
    this.ReadMore = this.acl.can('Root.Admin.Provide.ArticleEntities.ReadMore');
    this.http.get<AjaxResult>("api/Common/GetRegion",{level:2}).subscribe((res)=>{
      if(res.Data){
        res.Data.forEach(element=>{
          this.region.push({label:element.Name,value:element.Id});
        });
      }
    });


  }

 StatusTag = {
    0: { text: '未启用', color: '' },
    1: { text: '已启用', color: 'gold' },
    2: { text: '已关闭', color: '#f50' },
    3: { text: '已停用', color: 'red' },
  };

  protected ResponseDataProcess(data: STData[]): STData[] {
    this.data = data;
    return data;
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
                acl: 'Root.Admin.Provide.ArticleEntities.Update',
                iif: row => row.Updatable,
                click: row => this.redit(row),
              },
            ],
          },
        ],
      },
      {
        title: '编号',
        width: 100,
        index: 'Id',
        sort: true,
        readOnly: true,
        editable: true,
        filterable: true,
        ftype: 'string',
        acl: 'Root.Admin.Provide.ArticleEntities.ReadMore',
      },
      {
        title: '用户ID',
        width: 100,
        index: 'UserId',
        editable: true,
        filterable: true,
        type: 'number',
        acl: 'Root.Admin.Provide.ArticleEntities.ReadMore',
      },
      { title: '分类', width: 100, index: 'ArticleAssort_Name', ftype: 'string',filterable: true, },
      { title: '名称', width: 100, index: 'Name', editable: true, filterable: true, ftype: 'string' },
      { title: '备注', width: 100, index: 'Remarks', editable: true, filterable: true, ftype: 'string' },
      {
        title: '当前状态',
        index: 'Status',
        width: 100,
        editable: true,
        filterable: true,
        type: 'tag',
        tag: this.StatusTag,
        default: '0',
        render:'status',
      },
      { title: '详情', width: 100, index: 'Extra', ftype: 'string', render: 'extra' },

      { title: '创建者', width: 100, index: 'CreatorId', type: 'number',acl: 'Root.Admin.Provide.ArticleEntities.ReadMore', },
      { title: '创建时间', width: 100, index: 'CreatedTime', sort: true, filterable: true, type: 'date' },
      { title: '更新者', width: 100, index: 'LastUpdaterId', type: 'number' , acl: 'Root.Admin.Provide.ArticleEntities.ReadMore',},
      { title: '更新时间', width: 100, index: 'LastUpdatedTime', type: 'date' },
    ];

    return columns;
  }
  protected GetSFSchema(): SFSchema {
    let sch = { ArticleAssortId: null };
    sch.ArticleAssortId = { type: 'string', title: '目标分类' };
    let schema: SFSchema = {
      properties: sch,
      required: ['Name', 'Remarks', 'Status', 'UserId', 'ArticleAssortId'],
    };
    return schema;
  }

  protected redit(row) {
    if (!row || !this.editModal) {
      return;
    }

    this.schema = {
      properties: {
        UserId: {
          type: 'number',
          title: '用户ID',
          default: row.UserId,
          // tslint:disable-next-line: no-object-literal-type-assertion
          ui:{
            widget: 'select',
            placeholder:row.UserId
          } as SFSelectWidgetSchema
        },
        Name: {
          type: 'string',
          title: '名称',
          ui: {
            placeholder: row.Name,
          },
        },
        Remarks: {
          type: 'string',
          title: '备注',
          ui: {
            placeholder: row.Remarks,
          },
        },
        Status: {
          type: 'number',
          title: '状态',
          default: row.Status,
          // tslint:disable-next-line: no-object-literal-type-assertion
          ui: {
            widget: 'select',
            acl: 'Root.Admin.Provide.ArticleEntities.ReadMore',
            placeholder:row.Status
          } as SFSelectWidgetSchema,
        },
      },
      required: ['Name','Remarks','Status','UserId'],
    };
    this.ui = this.GetSFUISchema();
    this.editRow = row;
    this.isedit = true;
    this.editTitle = '编辑';
    this.editModal.open();
  }

   rsave(obj) {

    if(this.isedit){
      console.log(this.editRow);
      
      return;
    }

    const arrkey = ['ArticleAssortId', 'Name', 'Remarks', 'Status', 'UserId'];
    const sender: { Extra?; UserId?; ArticleAssortId?; Name?; Remarks?; Status? } = {};
    const extra = {};
    for (let key in obj) {
      if (obj.hasOwnProperty(key)) {
        if (!arrkey.find(a => a === key)) {
          extra[key] = obj[key];
        } else {
          sender[key] = obj[key];
        }
      }
    }
    sender.Extra = extra;
    const _http =
      sender.UserId && this.ReadMore ? this.api.CreateArticleEntity(sender) : this.pro.CreateArticleEntity(sender);
    _http.subscribe(r => {
      this.os.ajaxResult(
        r,
        () => {
          this.editModal.close();
          this.st.reload();
        },
        () => {},
      );
    });
  }

  protected GetSFUISchema(): SFUISchema {
    let ui: SFUISchema = {
      '*': { spanLabelFixed: 100, grid: { span: 12 } },
      $Name: { grid: { span: 24 } },
      $Remarks: { grid: { span: 24 } },
      $Status: {
        widget: 'select',
        asyncData: () => {
          const arr = [];
          if (this.isedit) {
            for (let key in this.StatusTag) {
              if (this.StatusTag.hasOwnProperty(key)) {
                arr.push({ label: this.StatusTag[key].text, value: key });
              }
            }
          } else {
            arr.push({ label: this.StatusTag[0].text, value: 0 });
          }
          return of(arr);
        },
      },
      $UserId: {
        widget: 'select',
        asyncData: () => this.api.ReadAllMerchant(2),
        acl: 'Root.Admin.Provide.ArticleEntities.ReadMore',
      },
      $ArticleAssortId: {
        grid: { span: 24 },
        widget: 'select',
        asyncData: () => {
          let read = this.ReadMore ? this.api.ReadCategory() : this.pro.ReadCategory();
          return read.map((r: any) => this.category(r));
        },
        change: (val: any) => {
          let read = this.ReadMore ? this.api.ReadChannelType(val) : this.pro.ReadChannelType(val);
          read.subscribe(r => {
            this.sfrefrsh(val, r);
          });
        },
      },
    };
    return ui;
  }

  protected category(val) {
    if (!val) {
      return [];
    }
    const arr = [];
    val.forEach(element => {
      arr.push({ label: `${element.Name}-${element.Channel_Name}`, value: element.Id });
    });
    return arr;
  }

  protected sfrefrsh(val, extra) {
    if (!extra) {
      return;
    }
    this.schema.properties.ArticleAssortId.default = val;
    if (this.ReadMore) {
      this.schema.properties.UserId = { type: 'number', title: '用户ID' };
    }
    this.schema.properties.Name = { type: 'string', title: '名称' };
    this.schema.properties.Remarks = { type: 'string', title: '备注' };
    for (let key in extra) {
      if (extra.hasOwnProperty(key)) {
        let typestr = this.gettype(extra[key].ValueType);
        let ui:SFUISchemaItem = { grid: { span: 24 } };
        if (typestr !== 'null') {
          if (typestr === 'number') ui.grid.span = 12;
          if (typestr === 'boolean') ui.grid.span = 12;
          if(typestr === 'areaenum'){
            ui.grid.span = 12;
            ui.widget = "select";
            // 区域选择
            this.schema.properties[key] = { title: key, type: "string",enum:this.region, ui };
          }else{
            this.schema.properties[key] = { title: key, type: typestr as SFSchemaType, ui };
          }
          this.schema.required.push(key);
        } else {
          this.schema.properties[key] = { title: key, type: 'string', ui };
        }
      }
    }

    this.schema.properties.Status = { type: 'number', title: '状态' };

    this.sf.refreshSchema();
  }

  protected gettype(i): string {
    switch (i) {
      case 1:
        return 'string';
      case 3:
        return 'boolean';
      case 2:
        return 'number';
      case 4:
        return 'null';
      case 8:
        return 'areaenum';
      default:
        return '未知';
    }
  }

   showextra(item, index) {
    let ownProps = Object.keys(item.Extra);
    let i = ownProps.length;
    let resArray = new Array(i);
    while (i--) resArray[i] = `${ownProps[i]}:${item.Extra[ownProps[i]]}`;
    this.showextrajson = resArray;
    this.extrashowmodal.open();
  }

   closeextra() {
    this.extrashowmodal.destroy();
  }

  // 切换账号状态
  tagclick(t){
    console.log(t);
  }
}
