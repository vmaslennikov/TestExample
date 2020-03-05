import { Observable } from 'rxjs';
import { ImageFile } from './../shared/models/image-file';
import { Folder } from './../shared/models/folder';
import { ImagesService } from './../shared/services/images.service';
import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';

import { FlatTreeControl } from '@angular/cdk/tree';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener
} from '@angular/material/tree';

interface FolderNode {
  path: string;
  name: string;
  root: string;
  children?: FolderNode[];
}
/** Flat node with expandable and level information */
interface FlatNode {
  expandable: boolean;
  name: string;
  path: string;
  root: string;
  level: number;
  children?: FolderNode[];
}

@Component({
  selector: 'app-view',
  templateUrl: './view.component.html',
  styleUrls: ['./view.component.css']
})
export class ViewComponent implements OnInit {
  constructor(private imagesService: ImagesService) {}

  @ViewChild('pageviewerbottom', {static: false}) elementView: ElementRef;

  dsFolders: Folder[] = [];
  dsFiles: ImageFile[] = [];
  dsFilesByPage: ImageFile[] = [];
  dsSelected: ImageFile[] = [];

  currentFolder: Folder;
  length = 0;
  pageSize = 9;
  currentPage = 1;
  TREE_DATA = [];

  private _transformer = (node: FolderNode, level: number) => {
    return {
      expandable: !!node.children && node.children.length > 0,
      name: node.name,
      path: node.path,
      root: node.root,
      level: level,
      children: node.children
    };
  }

  treeControl = new FlatTreeControl<FlatNode>(
    node => node.level,
    node => node.expandable
  );
  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.children
  );

  tree = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  hasChild = (_: number, node: FlatNode) => node.expandable;

  paginate(array, page_size, page_number) {
    this.dsFilesByPage = array.slice(
      (page_number - 1) * page_size,
      page_number * page_size
    );
  }

  onPaginateChange(event) {
    this.paginate(this.dsFiles, this.pageSize, event.pageIndex + 1);
  }

  ngOnInit() {
    this.updateFolders(null);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.updateImageWidth();
  }

  updateImageWidth() {
    if (this.dsSelected.length > 0) {
      const dimension = this.elementView.nativeElement.getBoundingClientRect();
      this.dsSelected.forEach(i => (i['width'] = (dimension.width * 0.35 + 'px')));
    }
  }


  toggleSelect(file) {
    if (file) {
      const selected = this.dsSelected.filter(o => o.path === file.path);
      if (selected && selected.length === 1) {
        file['selected'] = false;
        const index = this.dsSelected.indexOf(selected[0]);
        if (index !== -1) {
          this.dsSelected.splice(index, 1);
        }
      } else {
        const copy = Object.assign({}, file);
        file['selected'] = true;
        this.dsSelected.push(copy);
      }
      this.updateImageWidth();
    }
  }

  fillTree(array, data, level = 1) {
    data.forEach(o => {
      const item = {
        path: o['path'],
        name: o['name'],
        root: o['root'],
        children: [],
        level: level
      };
      array.push(item);
      if (o.children && o.children.length > 0) {
        this.fillTree(item.children, o.children, level + 1);
      }
    });
  }

  updateFolders(f) {
    this.imagesService.getFolders(null).subscribe((data: Folder[]) => {
      this.dsFolders.length = 0;
      data.forEach((o: Folder) => {
        this.dsFolders.push(o);
      });

      this.TREE_DATA = [
        {
          path: null,
          name: 'root',
          root: null,
          children: [],
          level: 0
        }
      ];
      this.fillTree(this.TREE_DATA[0].children, data, 1);
      this.tree.data = this.TREE_DATA;
    });
  }

  updateFiles(f) {
    if (!f) {
      return;
    }
    this.dsSelected.length = 0;
    if (this.currentFolder) {
      this.currentFolder['selected'] = false;
    }
    this.currentFolder = f;
    this.currentFolder['selected'] = true;

    this.imagesService.getFiles(f.path).subscribe((data: ImageFile[]) => {
      // console.log(JSON.stringify(data));
      this.dsFiles.length = 0;
      data.forEach((o: ImageFile) => {
        o['url'] = f.root + '/' + o['name'];
        this.dsFiles.push(o);
      });
      this.length = this.dsFiles.length;
      this.currentPage = 1;
      this.paginate(this.dsFiles, this.pageSize, this.currentPage);
    });
  }

  setBorder(f) {
    this.dsSelected.forEach(o => (o['selected'] = false));
    f['selected'] = true;
  }
}
