
function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.datagrid_read("grid1",index,1);
    
    
    s_ui.msg(a);
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    var data=s_sys.value_read("map");
    var strSplit=data.split("\n");
    
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",2,"数据,映射值");
    for (var i=0;i<strSplit.length;i++){
        var strSplit2=strSplit[i].split(",");
        s_ui.datagrid_add_line("grid1",strSplit2[0]+",",",");
    }
    //s_ui.datagrid_add_button("grid1","modify","映射","modify_click");
}



function save_click(data){
    
    var count=s_ui.datagrid_rows("grid1")-1;
    
    var strMap="";
    for (var i=0;i<count;i++){
        strMap+=s_ui.datagrid_read("grid1",i,0)+","
        +s_ui.datagrid_read("grid1",i,1)+"\n";
    }
    
    var row_index=s_sys.value_read("row_index");
    s_file.save(disk+"\\Net\\Web\\data\\map_c"+row_index+".txt",strMap);
    s_ui.close();
}

//界面
s_ui.datagrid_init("grid1",10,60,650,380);


s_ui.button_init("b_save","保存",100,450,200,30,"save_click","");


s_ui.show_form(700,600);
s_ui.Form_Title("3.map 字段映射文件");

data_init("");

