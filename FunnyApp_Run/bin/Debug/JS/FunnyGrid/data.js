

function data_init1(data){
    s_ui.DataGrid_Clear("grid1");
    
    s_ui.DataGrid_Init_Column("grid1",6,"A,B,C,D,E,F");
    s_ui.DataGrid_Add_Line("grid1","0,0",",");
    s_ui.DataGrid_Add_Line("grid1","1,2",",");
    s_ui.DataGrid_Add_Line("grid1","2,3",",");
    s_ui.DataGrid_Add_Line("grid1","3,3.5",",");
    s_ui.DataGrid_Add_Line("grid1","4,4",",");
    s_ui.DataGrid_Add_Line("grid1","5,4.5",",");
    s_ui.DataGrid_Add_Line("grid1","6,5",",");
    s_ui.DataGrid_Add_Line("grid1","7,6",",");

    s_ui.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}


function data_init2(data){
    s_ui.DataGrid_Clear("grid1");
    
    s_ui.DataGrid_Init_Column("grid1",6,"good,bad,WOE,D,E,F");
    s_ui.DataGrid_Add_Line("grid1","1,2",",");
    s_ui.DataGrid_Add_Line("grid1","2,3",",");
    s_ui.DataGrid_Add_Line("grid1","3,3",",");
    s_ui.DataGrid_Add_Line("grid1","4,4",",");
    s_ui.DataGrid_Add_Line("grid1","5,4",",");
    s_ui.DataGrid_Add_Line("grid1","6,5",",");
    s_ui.DataGrid_Add_Line("grid1","7,6",",");

    s_ui.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}


function data_init3(data){
    s_ui.DataGrid_Clear("grid1");
    
    s_ui.DataGrid_Init_Column("grid1",6,"good,bad,WOE,D,E,F");
    s_ui.DataGrid_Add_Line("grid1","50,40",",");
    s_ui.DataGrid_Add_Line("grid1","100,60",",");
    s_ui.DataGrid_Add_Line("grid1","100,80",",");
    s_ui.DataGrid_Add_Line("grid1","80,40",",");

    s_ui.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}




