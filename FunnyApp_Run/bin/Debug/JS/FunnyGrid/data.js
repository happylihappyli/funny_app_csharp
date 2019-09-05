

function data_init1(data){
    sys.DataGrid_Clear("grid1");
    
    sys.DataGrid_Init_Column("grid1",6,"A,B,C,D,E,F");
    sys.DataGrid_Add_Line("grid1","0,0",",");
    sys.DataGrid_Add_Line("grid1","1,2",",");
    sys.DataGrid_Add_Line("grid1","2,3",",");
    sys.DataGrid_Add_Line("grid1","3,3.5",",");
    sys.DataGrid_Add_Line("grid1","4,4",",");
    sys.DataGrid_Add_Line("grid1","5,4.5",",");
    sys.DataGrid_Add_Line("grid1","6,5",",");
    sys.DataGrid_Add_Line("grid1","7,6",",");

    sys.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}


function data_init2(data){
    sys.DataGrid_Clear("grid1");
    
    sys.DataGrid_Init_Column("grid1",6,"good,bad,WOE,D,E,F");
    sys.DataGrid_Add_Line("grid1","1,2",",");
    sys.DataGrid_Add_Line("grid1","2,3",",");
    sys.DataGrid_Add_Line("grid1","3,3",",");
    sys.DataGrid_Add_Line("grid1","4,4",",");
    sys.DataGrid_Add_Line("grid1","5,4",",");
    sys.DataGrid_Add_Line("grid1","6,5",",");
    sys.DataGrid_Add_Line("grid1","7,6",",");

    sys.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}


function data_init3(data){
    sys.DataGrid_Clear("grid1");
    
    sys.DataGrid_Init_Column("grid1",6,"A,B,C,D,E,F");
    sys.DataGrid_Add_Line("grid1","50,40",",");
    sys.DataGrid_Add_Line("grid1","100,60",",");
    sys.DataGrid_Add_Line("grid1","100,80",",");
    sys.DataGrid_Add_Line("grid1","80,40",",");

    sys.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}




