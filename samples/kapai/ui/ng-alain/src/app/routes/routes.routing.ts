import { NgModule, OnInit, Injector } from '@angular/core';
import { Routes, RouterModule, Router } from '@angular/router';
import { SimpleGuard } from '@delon/auth';
import { environment } from '@env/environment';
// layout
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { LayoutFullScreenComponent } from '../layout/fullscreen/fullscreen.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
import { ShoworderModule } from './showorder/showorder.module';

// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
import { ACLGuard } from '@delon/acl';

const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    canActivate: [SimpleGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: '仪表盘' } },
      { path: 'exception', loadChildren: () => import('./exception/exception.module').then(m => m.ExceptionModule) },

      // 要加载的业务模块
      { path: 'merchant', loadChildren: ()=> import('./merchant/merchant.module').then(m=>m.MerchantModule), canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Merchant' } },
      { path: 'provide', loadChildren: ()=> import('./provide/provide.module').then(m=>m.ProvideModule), canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Provide' } },
      { path: 'channel', loadChildren: ()=> import('./channel/channel.module').then(m=>m.ChannelModule), canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Channel' } },
      { path: 'cash-money', loadChildren: ()=> import('./cash-money/cash-money.module').then(m=>m.CashMoneyModule), canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Cash-money' } },

      // 业务子模块
      { path: 'identity', loadChildren: () => import('./identity/identity.module').then(m => m.IdentityModule) },
      { path: 'security', loadChildren: () => import('./security/security.module').then(m => m.SecurityModule) },
      { path: 'systems', loadChildren: () => import('./systems/systems.module').then(m => m.SystemsModule) },
      { path: 'infos', loadChildren: () => import('./infos/infos.module').then(m => m.InfosModule), canActivateChild: [ACLGuard], data: { guard: 'Root.Admin.Infos' } },
    ]
  },
  // 全屏布局
  // {
  //     path: 'fullscreen',
  //     component: LayoutFullScreenComponent,
  //     children: [
  //     ]
  // },
  // passport
  {
    path: 'passport',
    component: LayoutPassportComponent,
    loadChildren: () => import('./passport/passport.module').then(m => m.PassportModule)
  },
  // 单页不包裹Layout
  { path: 'callback/:type', component: CallbackComponent },
  { path: 'payorder',loadChildren:()=>import('./showorder/showorder.module').then(m=>m.ShoworderModule)},
  { path: '**', redirectTo: 'exception/404' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      routes, {
      useHash: environment.useHash,
      // NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
      // Pls refer to https://ng-alain.com/components/reuse-tab
      scrollPositionRestoration: 'top',
    }
    )],
  exports: [RouterModule],
})
export class RouteRoutingModule  {

}
