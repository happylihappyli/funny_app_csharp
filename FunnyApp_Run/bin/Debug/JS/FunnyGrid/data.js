

function data_init1(data){
    sys.DataGrid_Clear("grid1");
    
    sys.DataGrid_Init_Column("grid1",6);
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
    
    sys.DataGrid_Init_Column("grid1",6);
    sys.DataGrid_Add_Line("grid1","1,2",",");
    sys.DataGrid_Add_Line("grid1","2,3",",");
    sys.DataGrid_Add_Line("grid1","3,3",",");
    sys.DataGrid_Add_Line("grid1","4,4",",");
    sys.DataGrid_Add_Line("grid1","5,4",",");
    sys.DataGrid_Add_Line("grid1","6,5",",");
    sys.DataGrid_Add_Line("grid1","7,6",",");

    sys.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}