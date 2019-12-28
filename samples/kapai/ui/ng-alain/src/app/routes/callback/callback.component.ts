import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SocialService } from '@delon/auth';
import { SettingsService, ScrollService } from '@delon/theme';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  providers: [SocialService],
  styles: [`
  .listbdc {
    position:absolute;
    height:260px;
    width:553px;
    overflow:hidden;
    background:#8181F7;
    overflow-y: scroll;
  }
  `],
})
export class CallbackComponent implements OnInit {
  type: string;
  list:any[];

  
  constructor(
    private socialService: SocialService,
    private settingsSrv: SettingsService,
    private route: ActivatedRoute,
    private scr:ScrollService,
  ) {}

  ngOnInit(): void {
    this.type = this.route.snapshot.params.type;
    this.mockModel();

  }

  private mockModel() {
    this.list = Object.keys(Array.from({ length: 20 }));
    /*
    this.settingsSrv.setUser({
      ...this.settingsSrv.user,
      ...info,
    });
    this.socialService.callback(info);*/
  }

  del(index:number){
    this.list.splice(index,1);
  }

}
