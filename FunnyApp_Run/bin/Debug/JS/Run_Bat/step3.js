
function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.DataGrid_Read("grid1",index,1);
    
    s_ui.Run_JS("Run_Bat\\step3_map.js");
    //s_ui.Msg(a);
}

function clear_data(data){
    s_ui.DataGrid_Clear("grid1");
}

function data_init(data){
    s_ui.DataGrid_Clear("grid1");
    
    s_ui.DataGrid_Init_Column("grid1",3,"字段,类型,C");
    s_ui.DataGrid_Add_Line("grid1","1,数字",",");
    s_ui.DataGrid_Add_Line("grid1","2,字符串",",");
    s_ui.DataGrid_Add_Line("grid1","3,数字",",");
    s_ui.DataGrid_Add_Line("grid1","4,数字",",");
    s_ui.DataGrid_Add_Line("grid1","5,字符串",",");
    s_ui.DataGrid_Add_Line("grid1","6,数字",",");
    s_ui.DataGrid_Add_Line("grid1","7,字符串",",");
    
    s_ui.datagrid_add_button("grid1","modify","映射","modify_click");
}

function static_click(data){
    
}


function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

//界面
s_ui.DataGrid_Init("grid1",10,60,750,380);




s_ui.Button_Init("b_static","重新统计分析",100,450,200,30,"static_click","");

s_ui.Button_Init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat\\step2");
s_ui.Button_Init("b_next","下一步",350,500,200,30,"next_click","Run_Bat\\step4");


s_ui.Show_Form(800,600);
s_ui.Form_Title("第3步 字段映射");

data_init("");

