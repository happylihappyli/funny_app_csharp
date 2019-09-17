
var userName="none";
var log_msg="";

function save_check_click(data){

    var file=s_ui.Text_Read("txt_file");
    var user=s_ui.Text_Read("txt_user");
    var group=s_ui.Text_Read("txt_group");
    //chown runoob:runoobgroup file1.txt
    s_sys.Value_Save("cmd","chown "+user+":"+group+" "+file);
    
    s_ui.close();
}

var file=s_sys.Value_Read("file");

s_ui.Label_Init("lb_memo","修改文件目录用户组",150,30);

s_ui.Label_Init("lb_file","文件(目录)名:",10,100);
s_ui.Text_Init("txt_file",file,150,100,300,30);

s_ui.Label_Init("lb_user","用户:",10,150);
s_ui.Text_Init("txt_user","",150,150,300,30);

s_ui.Label_Init("lb_group","用户组:",10,200);
s_ui.Text_Init("txt_group","",150,200,300,30);

s_ui.Button_Init("b1_save","保存",150,250,100,30,"save_check_click","");


//其他属性
s_ui.Acception_Button("b1_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("修改用户组");
