import { Injectable, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { AjaxResult, AjaxResultType, CashDto } from '../osharp.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { OsharpService } from './osharp.service';

@Injectable({
  providedIn: 'root',
})
/**
 *  供应商通用APi
 * 
 */
export class ProvideService {
  constructor(private injector: Injector, public http: _HttpClient,protected os:OsharpService) {}


  /**
   *  获取通道 参数模板
   * @param id  分类Id
   */
  ReadChannelType(id:string):Observable<any>{
    const url = 'api/Provide/ProvideOpen/ReadChannelType';
    return this.http.get<AjaxResult>(url,{articleAssortId:id}).map(r=>r.Data);
  }

  /**
   *  获取所有通道以分类
   */
  ReadCategory():Observable<any>{
    const url = 'api/Provide/ProvideOpen/ReadCategory';
    return this.http.get<AjaxResult>(url).map(r=>r.Data);
  }

  CreateArticleEntity(send:{ Extra?,UserId?,ArticleAssortId?,Name?,Remarks?,Status?}):Observable<any>{
    const url = 'api/Provide/ProvideOpen/CreateArticleEntity';
    return this.http.post<AjaxResult>(url,[send]);
   
  }
}
