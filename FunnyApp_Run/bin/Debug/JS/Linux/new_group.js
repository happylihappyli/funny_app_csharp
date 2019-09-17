
var userName="none";
var log_msg="";

function save_check_click(data){

    var name=s_ui.Text_Read("txt_name");
    s_sys.Value_Save("cmd","groupadd "+name);
    //s_ui.Msg("保存成功！");
    s_ui.close();
}


s_ui.Label_Init("lb1","添加用户组",150,30);

s_ui.Label_Init("lb2","用户组名:",10,100);
s_ui.Text_Init("txt_name","",150,100,300,30);


s_ui.Button_Init("b1_save","保存",150,300,100,30,"save_check_click","");


//其他属性
s_ui.Acception_Button("b1_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("添加用户组");
