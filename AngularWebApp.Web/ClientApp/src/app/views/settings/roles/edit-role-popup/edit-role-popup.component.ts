import { Component, OnInit } from '@angular/core';
import { Role } from '../../../../services/roles.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'app-edit-role-popup',
  templateUrl: './edit-role-popup.component.html'
})
export class EditRolePopupComponent implements OnInit {
  public role: Role;
  public onCommand: Subject<string>;

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit() {
    this.onCommand = new Subject<string>();
  }

  ok() {
    this.bsModalRef.hide();
    this.onCommand.next("ok");
  }

  cancel() {
    this.bsModalRef.hide();
    this.onCommand.next("cancel");
  }
}
