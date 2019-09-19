
function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.datagrid_read("grid1",index,10);
    
    //s_ui.Run_JS("Run_Bat\\step3_map.js");
    //s_ui.Msg(a);
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",10,"字段,非*个数,均值,方差,0%,25%,50%,75%,100%,*占比(缺失)");
    s_ui.datagrid_add_line("grid1","1,10,5.2,0.1,1,3,8,10,16,0",",");
    s_ui.datagrid_add_line("grid1","2,10,5.2,0.1,1,3,8,10,16,0",",");
    s_ui.datagrid_add_line("grid1","3,10,5.2,0.1,1,3,8,10,16,0",",");
    s_ui.datagrid_add_line("grid1","4,10,5.2,0.1,1,3,8,10,16,0",",");
    s_ui.datagrid_add_line("grid1","5,10,5.2,0.1,1,3,8,10,16,0",",");
    s_ui.datagrid_add_line("grid1","6,10,5.2,0.1,1,3,8,10,16,0",",");
    s_ui.datagrid_add_line("grid1","7,10,5.2,0.1,1,3,8,10,16,0",",");
    
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
    s_ui.Msg(line);
}


function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

//界面
s_ui.datagrid_init("grid1",10,60,750,380);




s_ui.Button_Init("b_static","保存",100,450,200,30,"save_click","");

s_ui.Button_Init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat\\step3");
s_ui.Button_Init("b_next","下一步",350,500,200,30,"next_click","Run_Bat\\step5");


s_ui.Show_Form(800,600);
s_ui.Form_Title("第4步 字段统计(*代表缺失数据)");

data_init("");

