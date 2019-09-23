
function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.datagrid_Read("grid1",index,1);
    
    
    s_ui.msg(a);
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",2,"数据,映射值");
    s_ui.datagrid_add_line("grid1","A,",",");
    s_ui.datagrid_add_line("grid1","B,",",");
    s_ui.datagrid_add_line("grid1","C,",",");
    s_ui.datagrid_add_line("grid1","D,",",");
    
    //s_ui.datagrid_add_button("grid1","modify","映射","modify_click");
}

function static_click(data){
    
}


function save_click(data){
    s_ui.close();
}

//界面
s_ui.datagrid_init("grid1",10,60,650,380);


s_ui.button_init("b_save","保存",100,450,200,30,"save_click","");


s_ui.Show_Form(700,600);
s_ui.Form_Title("3.map 字段映射文件");

data_init("");

