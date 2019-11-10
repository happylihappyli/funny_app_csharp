
var userName="none";
var log_msg="";

function save_check_click(data){

    var name=s_ui.text_read("txt_name");
    var group=s_ui.text_read("txt_group");
    
    s_sys.value_save("cmd","usermod -a -G "+group+" "+name);
    //s_ui.msg("保存成功！");
    s_ui.close();
}


s_ui.label_init("lb1","添加用户到组",150,30);

s_ui.label_init("lb_name","用户名:",10,100);
s_ui.text_init("txt_name","",150,100,300,30);
s_ui.label_init("lb_group","组名:",10,150);
s_ui.text_init("txt_group","",150,150,300,30);


s_ui.button_init("b1_save","保存",150,300,100,30,"save_check_click","");


//其他属性
s_ui.button_default("b1_save");
s_ui.show_form(560,380);
s_ui.Form_Title("添加用户到组");
