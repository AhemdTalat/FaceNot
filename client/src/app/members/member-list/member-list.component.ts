import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  userParam: UserParams;
  user: User;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]

  constructor(private memberService: MembersService) {
    this.userParam = memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.setUserParam(this.userParam);
    this.memberService.getMembers(this.userParam).subscribe(respone => {
      this.pagination = respone.pagination;
      this.members = respone.result;
    })
  }

  resetFilters() {
    this.userParam = this.memberService.resetUserParam();
    this.loadMember();
  }

  pageChanged(event: any) {
    this.userParam.pageNumber = event.page;
    this.memberService.setUserParam(this.userParam);
    this.loadMember();
  }
}
