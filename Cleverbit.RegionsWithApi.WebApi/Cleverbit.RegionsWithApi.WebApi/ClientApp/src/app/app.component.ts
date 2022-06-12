import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Regions With Api';

  employees: any[] = [];

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.fetchEmplyees(150).subscribe(data => {
      console.log(data);
      });
  }

  fetchEmplyees(regionId: number) {
    let url = `https://localhost:44385/api/regions/${regionId}/employees`;
    return this.http.get<any[]>(url);
  }
}
