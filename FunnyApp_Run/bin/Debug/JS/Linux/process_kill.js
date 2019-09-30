
var userName="none";
var log_msg="";

[[[..\\data\\common_string.js]]]

function save_check_click(data){

    var file1=s_ui.combox_text("cb_file");
    
    var cmd="ps aux|grep "+file1+"|get_col 2|xargs kill -9";
    s_sys.value_save("cmd",cmd);
    s_ui.close();
}


s_ui.label_init("lb1","删除进程",10,30);

s_ui.label_init("lb_file","输入进程名:",10,150);
s_ui.combox_init("cb_file","",150,150,300,30);
s_ui.combox_add("cb_file","file_sql");


s_ui.button_init("b_save","保存",150,300,100,30,"save_check_click","");


//其他属性
s_ui.acception_button("b_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("删除进程");
