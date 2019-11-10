
function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.datagrid_read("grid1",index,1);
    
    //s_ui.Run_JS("Run_Bat\\step3_map.js");
    //s_ui.msg(a);
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",1,"字段");
    s_ui.datagrid_add_line("grid1","1",",");
    s_ui.datagrid_add_line("grid1","2",",");
    s_ui.datagrid_add_line("grid1","3",",");
    s_ui.datagrid_add_line("grid1","4",",");
    s_ui.datagrid_add_line("grid1","5",",");
    s_ui.datagrid_add_line("grid1","6",",");
    s_ui.datagrid_add_line("grid1","7",",");
    
    s_ui.datagrid_add_checkbox("grid1","modify","选择","modify_click");
}

function save_click(data){
    var line="";
    var count=s_ui.datagrid_rows("grid1")-1;
    for (var i=0;i<count;i++){
    //var index=parseInt(data);
    var a=s_ui.datagrid_read("grid1",i,10);
    line+=a;
    }
    s_ui.msg(line);
}


function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

//界面

s_ui.label_init("lb_tip","选择y字段",10,10);
s_ui.datagrid_init("grid1",10,60,350,380);


s_ui.label_init("lb_good","好样本",500,60);

s_ui.label_init("lb_bad","坏样本",500,160);


s_ui.button_init("b_static","保存",100,450,200,30,"save_click","");

s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat\\step3");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat\\step5");


s_ui.show_form(800,600);
s_ui.Form_Title("第5步 定义好坏样本");

data_init("");

