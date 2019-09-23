
var userName="none";
var log_msg="";

function save_check_click(data){

    var name=s_ui.text_read("txt_name");
    s_sys.value_save("cmd","groupadd "+name);
    //s_ui.msg("保存成功！");
    s_ui.close();
}


s_ui.label_init("lb1","添加用户组",150,30);

s_ui.label_init("lb2","用户组名:",10,100);
s_ui.text_init("txt_name","",150,100,300,30);


s_ui.button_init("b1_save","保存",150,300,100,30,"save_check_click","");


//其他属性
s_ui.acception_button("b1_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("添加用户组");
