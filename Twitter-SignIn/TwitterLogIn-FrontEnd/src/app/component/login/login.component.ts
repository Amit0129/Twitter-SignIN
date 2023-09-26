import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestToken } from 'src/app/model/RequestToken.model';
import { TwitterAuthService } from 'src/app/service/twitter-auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private requestToken: Partial<RequestToken> = {};
   disableButton = false;
   isLoading = false;

   constructor(private twitterService: TwitterAuthService,   private route: ActivatedRoute, private router: Router) { }

   ngOnInit() {

   }

   launchTwitterLogin() {
    this.isLoading = true;
    this.disableButton = true;
    this.twitterService.getRequestToken()
     .subscribe(response => this.requestToken = response, 
       error => console.log(error), 
       () => {
       location.href = "https://api.twitter.com/oauth/authenticate?" + this.requestToken.oauth_token;
       }
     );
    }
}
