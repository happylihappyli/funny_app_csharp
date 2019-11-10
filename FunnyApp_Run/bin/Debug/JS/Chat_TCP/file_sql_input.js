
var userName="none";
var log_msg="";

[[[..\\data\\common_string.js]]]

function save_check_click(data){

    var user_name=s_ui.text_read("txt_user");
    var file1=s_ui.combox_text("cb_file");
    var strMax=s_ui.text_read("txt_max");
    var sql=s_ui.text_read("txt_sql");
    var sep=s_ui.text_read("txt_sep");
    var file2=s_ui.combox_text("cb_output");
    
    sql=sql.replaceAll("\r"," ");
    sql=sql.replaceAll("\n"," ");
    
    var cmd="file_sql "+user_name+" "+file1+" "+strMax+" \""+sql+"\" "+sep+" "+file2;
    s_sys.value_save("cmd",cmd);
    //s_ui.msg("保存成功！");
    s_ui.close();
}


s_ui.label_init("lb1","文件SQL查询",10,30);

s_ui.label_init("lb_user","用户名:",10,100);
s_ui.text_init("txt_user","x",150,100,300,30);

s_ui.label_init("lb_file","输入文件:",10,150);
s_ui.combox_init("cb_file","",150,150,300,30);
s_ui.combox_add("cb_file","/home/ftp_home/upload/sample1.txt");
s_ui.combox_add("cb_file","/root/test1.txt");

s_ui.label_init("lb_max","最大个数:",10,200);
s_ui.text_init("txt_max","250000",150,200,300,30);

s_ui.label_init("lb_sql","SQL:",10,250);
s_ui.textbox_init("txt_sql","",150,250,300,60);

s_ui.label_init("lb_sep","分隔符",10,350);
s_ui.text_init("txt_sep","v",150,350,300,30);

s_ui.label_init("lb_output","输出文件:",10,400);

s_ui.combox_init("cb_output","",150,400,300,30);
s_ui.combox_add("cb_output","/root/test1.txt");
s_ui.combox_add("cb_output","/root/test2.txt");


s_ui.button_init("b_save","保存",150,500,100,30,"save_check_click","");


//其他属性
//s_ui.button_default("b_save");
s_ui.show_form(560,580);
s_ui.Form_Title("文件SQL查询");
