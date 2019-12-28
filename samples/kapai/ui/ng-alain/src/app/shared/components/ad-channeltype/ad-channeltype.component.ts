import { AlainService } from '@shared/osharp/services/alain.service';
import { Component, OnInit, Input, Output, EventEmitter, ViewChild, Injector } from '@angular/core';
import { OsharpService } from '@shared/osharp/services/osharp.service';
import { NzModalComponent, NzMessageService } from 'ng-zorro-antd';
import { FilterRule, FilterOperate, PageRequest, FilterGroup, AjaxResult } from '@shared/osharp/osharp.model';
import { List } from 'linqts';
import { OsharpSTColumn } from '@shared/osharp/services/alain.types';
import { ApiService } from '@shared/osharp/services/api.service';
import { _HttpClient } from '@delon/theme';

//#region 额外类型
export class ChannelType {
  Node: TypeNode[] = [];
  Id: any;
}

export enum ValueTypeEnum {
  String = 1,
  Number = 2,
  Boolean = 3,
  Null = 4,
  Undefined = 5,
  Object = 6,
  Array = 7,
  AreaEnum = 8,
}
export class ValueTypeEnumEntries {
  ValueType: ValueTypeEnum;
  Display: string;
  constructor(valueType: ValueTypeEnum) {
    switch (valueType) {
      case ValueTypeEnum.String:
        this.Display = '文本';
        break;
      case ValueTypeEnum.Array:
        this.Display = 'Array';
        break;
      case ValueTypeEnum.Boolean:
        this.Display = '逻辑';
        break;
      case ValueTypeEnum.Null:
        this.Display = '可空';
        break;
      case ValueTypeEnum.Number:
        this.Display = '数字';
        break;
      case ValueTypeEnum.Object:
        this.Display = 'Object';
        break;
      case ValueTypeEnum.Undefined:
        this.Display = 'Undefined';
        break;
      case ValueTypeEnum.AreaEnum:
          this.Display = '区域';
        break;
      default:
        this.Display = '未知';
        break;
    }
    this.ValueType = valueType;
  }
}

export class TypeNode {
  /** 属性名 */
  Field: string;
  /** 属性值 */
  Value: any;
  /** 属性值类型 */
  ValueType: ValueTypeEnum;

  constructor(field: string, value: string, valuetype: ValueTypeEnum = ValueTypeEnum.String) {
    this.Field = field;
    this.Value = value;
    this.ValueType = valuetype;
  }
  [key: string]: any;
}
//#endregion

@Component({
  selector: 'osharp-ad-channeltype',
  templateUrl: './ad-channeltype.component.html',
  styles: [
    `
      .node-box {
        margin: 2px;
        padding: 3px;
        border: dashed 2px #ddd;
      }
    `,
  ],
})
export class AdchanneltypeComponent implements OnInit {
  title = '参数模板';
  @ViewChild('newmodal', { static: false }) newmodal: NzModalComponent;

  @Input() updateUrl: string;
  channels: any[] = [];
  channel: any;
  channeltype: ChannelType;
  visible: boolean;
  region:any[] = [];
  @Input() createUrl: string;
  @Output() Success:EventEmitter<any> = new EventEmitter();
  channeltypeEntries: ValueTypeEnumEntries[] = [
    new ValueTypeEnumEntries(ValueTypeEnum.String),
    new ValueTypeEnumEntries(ValueTypeEnum.Boolean),
    new ValueTypeEnumEntries(ValueTypeEnum.Number),
    new ValueTypeEnumEntries(ValueTypeEnum.Null),
    new ValueTypeEnumEntries(ValueTypeEnum.AreaEnum),
  ];
  constructor(
    injector: Injector,
    protected api: ApiService,
    protected msg: NzMessageService,
    protected osharp: OsharpService,
  ) {}
  get http(): _HttpClient {
    return this.osharp.http;
  }

  ngOnInit(): void {
    this.channeltype = new ChannelType();
    this.http.get<AjaxResult>("api/Common/GetRegion",{level:2}).subscribe((res)=>{
      if(res.Data){
        this.region = res.Data
      }
    });
  }

  public opennewmodel() {
    this.GetChannel(true);
    this.newmodal.open();
  }

  public closenewmodel() {
    this.resetnewmodel(); // 重置
    this.newmodal.close();
  }

  public resetnewmodel() {
    this.channel = null;
    this.channeltype.Node = [];
  }

  public submitnewmodel() {
    const loc = {};
    try {
      this.channeltype.Node.some((v, i) => {
        if (!loc.hasOwnProperty(v.Field)) loc[v.Field] = { Value: v.Value, ValueType: v.ValueType };
        else throw new Error(`参数名:${v.Field},重复!请更改`);
      });

      if (!this.channel) {
        throw new Error(`未选择通道`);
      }
      let obj;
      let url;
      if (this.channeltype.Id) {
        obj = { Id: this.channeltype.Id, ChannelId: this.channel, ChannelJson: loc };
        url = this.updateUrl;
      } else {
        obj = { ChannelId: this.channel, ChannelJson: loc };
        url = this.createUrl;
      }
      this.http.post(url, [obj]).subscribe(r => {
        this.osharp.ajaxResult(r, () => {
          this.closenewmodel(); // 重置
          this.Success.emit(obj);
        });
      });
    } catch (error) {
      this.msg.warning(error);
    }
  }

  public removeNode(index: number, node: TypeNode, channel: ChannelType) {
    channel.Node.splice(index, 1);
  }

  public addNode(channel: ChannelType) {
    channel.Node.push(new TypeNode('参数名', '参数内容'));
  }

  public GetChannel(open: boolean) {
    if (!open) return;
    this.api.ReadChannels().subscribe(data => {
      this.channels = data;
    });
  }

  public ChannelJsonnShow(val: any,edit:boolean = true) {
    const arr = [];
    for (let k in val.ChannelJson) {
      if (val.ChannelJson.hasOwnProperty(k))
        arr.push(new TypeNode(k, val.ChannelJson[k].Value, val.ChannelJson[k].ValueType));
    }
    this.channeltype.Node = arr;
    this.channel = val.ChannelId;
    this.channeltype.Id = val.Id;
    this.opennewmodel();
  }
}
