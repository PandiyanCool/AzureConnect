import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { MyTableDataSource } from './app-datasource';
import { IEvent } from './shared';
import { Observable } from 'rxjs';
import { DataService } from './shared/services/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private dataService: DataService) { }

  [x: string]: any;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: MyTableDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['Level', 'ResultType', 'Time', 'ResultSignature'];


  ngOnInit() {

    this.dataService.getEventList().subscribe(x => {
      this.result = x;
      console.log(this.result);
      this.dataSource = new MyTableDataSource(this.paginator, this.sort, this.dataService);
      this.dataSource.data = x;
    });


  }


}
