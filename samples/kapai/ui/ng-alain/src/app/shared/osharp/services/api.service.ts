import { Injectable, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { Observable } from 'rxjs';
import { OsharpService } from './osharp.service';
import { AjaxResult } from '../osharp.model';

@Injectable({
  providedIn: 'root',
})
/**
 * 后台通用api
 */
export class ApiService {
  constructor(private injector: Injector, public http: _HttpClient,private osharp: OsharpService,) {}

  /**
   * 获取所有通道列表
   */
  ReadChannels(): Observable<any> {
    const url = 'api/Admin/FunctionApi/ReadChannels';
    return this.http.post<AjaxResult>(url).map(r=>r.Data);
  }
  /**
   * 获取所有用户ID 0 为所有用户 1为商户 2为供应商
   */
  ReadAllMerchant(index:number = 0): Observable<any> {
    const url = 'api/Admin/FunctionApi/ReadAllMerchant';
    return this.http.get<AjaxResult>(url,{type:index}).map(a=>a.Data);
  }

  /**
   *  获取对应用户 未设置的费率
   * @param userid 用户ID
   */
  ReadPercentageName(userId:number,channelId:string):Observable<any>{
    const url = 'api/Admin/FunctionApi/ReadPercentageName';
    return this.http.get<AjaxResult>(url,{userId,channelId}).map((a:any)=>{
      return a.Data;
    });
  }

  /**
   *  获取所有通道以分类
   */
  ReadCategory():Observable<any>{
    const url = 'api/Admin/FunctionApi/ReadCategory';
    return this.http.get<AjaxResult>(url).map(r=>r.Data);
  }
  /**
   *  获取参数模板
   */
  ReadChannelType(id:string):Observable<any>{
    const url = 'api/Admin/FunctionApi/ReadChannelType';
    return this.http.get<AjaxResult>(url,{articleAssortId:id}).map(r=>r.Data);
  }

  CreateArticleEntity(send:{ Extra?,UserId?,ArticleAssortId?,Name?,Remarks?,Status?}):Observable<any>{
    const url = 'api/Admin/FunctionApi/CreateArticleEntity';
    return this.http.post<AjaxResult>(url,[send]).map(r=>r.Data);
  }
}
