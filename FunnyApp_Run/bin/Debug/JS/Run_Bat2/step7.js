
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]

function Node_Click(data){
    
}

function check_click(data){
    var index=parseInt(data);
    var k=index+1;
    var a=s_ui.datagrid_read("grid1",index,1);
    //s_ui.msg(a);
    if (a=="1"){
        var file=disk+"\\Net\\Web\\Data\\memo.ini";
        s_file.Ini_Save(file,"y","index",k);
    }
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",1,"字段");
    
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    var count=parseInt(s_file.Ini_Read(file,"main","count"));
    for (var i=1;i<=count;i++){
        s_ui.datagrid_add_line("grid1",i+",",",");
    }
    s_ui.datagrid_add_checkbox("grid1","check1","选择","check_click");
    
    var k=parseInt(s_file.Ini_Read(file,"y","index"))-1;
    s_ui.datagrid_set_checkbox("grid1",k,1,"1");
}



function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

function read_ini(data){
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    
    var c1=s_file.Ini_Read(file,"good","compare");
    s_ui.combox_select("combox_good",c1);
    
    var good=s_file.Ini_Read(file,"good","value");
    s_ui.text_set("txt_good",good);
    
    var good_map=s_file.Ini_Read(file,"good","map");
    s_ui.combox_text_set("combox_good_map",good_map);
    
    
    var c2=s_file.Ini_Read(file,"bad","compare");
    s_ui.combox_select("combox_bad",c2);
    
    var bad=s_file.Ini_Read(file,"bad","value");
    s_ui.text_set("txt_bad",bad);
    
    var bad_map=s_file.Ini_Read(file,"bad","map");
    s_ui.combox_text_set("combox_bad_map",bad_map);
    
}

function save_click(data){
    
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    
    var c1=s_ui.combox_index("combox_good");
    s_file.Ini_Save(file,"good","compare",c1);
    
    var good=s_ui.text_read("txt_good");
    s_file.Ini_Save(file,"good","value",good);
    
    var good_map=s_ui.combox_text("combox_good_map");
    s_file.Ini_Save(file,"good","map",good_map);
    
    var c2=s_ui.combox_index("combox_bad");
    s_file.Ini_Save(file,"bad","compare",c2);
    
    var bad=s_ui.text_read("txt_bad");
    s_file.Ini_Save(file,"bad","value",bad);
    
    var bad_map=s_ui.combox_text("combox_bad_map");
    s_file.Ini_Save(file,"bad","map",bad_map);
    
}


//界面

s_ui.label_init("lb_tip","选择y字段",10,10);
s_ui.datagrid_init("grid1",10,60,350,380);


s_ui.label_init("lb_good","好样本",400,60);

s_ui.combox_init("combox_good",">",400,110,50,30);
s_ui.combox_add("combox_good",">");
s_ui.combox_add("combox_good",">=");
s_ui.combox_add("combox_good","=");
s_ui.combox_add("combox_good","<");
s_ui.combox_add("combox_good","<=");
s_ui.combox_add("combox_good","<>");

s_ui.text_init("txt_good","",470,110,100,30);
s_ui.label_init("lb_good_map","映射为",600,60);
s_ui.combox_init("combox_good_map","1",600,110,50,30);
s_ui.combox_add("combox_good_map","1");
s_ui.combox_add("combox_good_map","0");



s_ui.label_init("lb_bad","坏样本",400,210);
s_ui.combox_init("combox_bad",">",400,250,50,30);
s_ui.combox_add("combox_bad",">");
s_ui.combox_add("combox_bad",">=");
s_ui.combox_add("combox_bad","=");
s_ui.combox_add("combox_bad","<");
s_ui.combox_add("combox_bad","<=");
s_ui.combox_add("combox_bad","<>");

s_ui.text_init("txt_bad","",470,250,100,30);
s_ui.label_init("lb_bad_map","映射为",600,210);
s_ui.combox_init("combox_bad_map","0",600,250,50,30);
s_ui.combox_add("combox_bad_map","0");
s_ui.combox_add("combox_bad_map","1");


s_ui.button_init("b_static","保存",400,350,200,30,"save_click","");

s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step6");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step8");


s_ui.Show_Form(800,600);
s_ui.Form_Title("v2 第7步 定义好坏样本");

sys_read_ini("");

data_init("");

read_ini("");
