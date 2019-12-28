import { Injectable, Injector } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { AjaxResult, AjaxResultType, CashDto } from '../osharp.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MerchantService {
  constructor(private injector: Injector, public http: _HttpClient) {}

  /**
   * 获取收款卡列表 (单选)
   */
  ReadBank(): Observable<any> {
    const url = 'api/Merchant/MerchantOpen/ReadBank';
    return this.http.post<AjaxResult>(url).pipe(
      map((val: any) => {
        return val.Data;
      }),
    );
  }
  /**
   * 获取通道费率
   * @param channelid 要获取费率的通道ID
   */
  ReadChannelPer(channelid: string): Observable<any> {
    const url = 'api/Merchant/MerchantOpen/ReadChannelPer';
    return this.http.post<AjaxResult>(url, channelid).pipe(
      map((val: any) => {
        return val.Data;
      }),
    );
  }
  /**
   * 读取开通的通道 列表
   */
  ReadChannel(): Observable<any> {
    const url = 'api/Merchant/MerchantOpen/ReadChannel';
    return this.http.post<AjaxResult>(url).pipe(
      map((val: any) => {
        return val.Data;
      }),
    );
  }
  /**
   * 获取下级用户列表
   */
  ReadSubordinate(): Observable<any> {
    const url = 'api/Merchant/MerchantOpen/ReadSubordinate';
    return this.http.post<AjaxResult>(url).pipe(
      map((val: any) => {
        return val.Data;
      }),
    );
  }

  /**
   *  读取余额
   */
  ReadPoint(): Observable<any> {
    const url = 'api/Merchant/MerchantOpen/ReadPoint';
    return this.http.post<AjaxResult>(url).pipe(
      map((val: any) => {
        return val.Data;
      }),
    );
  }

  /**
   * 读取用户信息
   */
  Read(): Observable<any> {
    const url = 'api/Merchant/MerchantOpen/Read';
    return this.http.post<AjaxResult>(url).pipe(
      map((val: any) => {
        return val.Data;
      }),
    );
  }
  /**
   *  商户提现 返回 AjaxResultType
   *  
   */
  Cash(dto:CashDto):Observable<AjaxResult>{
    const url = 'api/Merchant/MerchantOpen/Cash';
    return this.http.post<AjaxResult>(url,[dto]);
  }
}
