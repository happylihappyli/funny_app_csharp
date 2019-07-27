
var userName="test";
//按钮点击
function run_click(data){
    var id="21";
    var param="1111111111111111111111111111";
    var url="http://www.funnyai.com/funnyscript/fs_run_line_shell.php?id="+id+"&user="
+encodeURIComponent(userName)+"&param="+encodeURIComponent(param);
    var result=sys.Net_Http_GET(url);
    show_error(result);
}

function set_status(data){
    var strSplit=data.split(",");
    sys.Show_ProgressBar("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    sys.Show_Text("txt_error",log_error);
}


function send_msg_click(){
    sys.Send_Msg("chat_event","csharp","*",sys.Get_Text("send_msg"));
}

var log_error="";
function event_connected(data){
    log_error+="Connected\r\n";
    sys.Show_Text("txt_error",log_error);
}


function event_chat(data1){
    var data=JSON.parse(data1);
    log_error=data.message+"\r\n"+log_error;
    sys.Show_Text("txt_error",log_error);
}


function event_system(data1){
    var data=JSON.parse(data1);
    log_error=data.from+"："+data.message+"\r\n"+log_error;
    sys.Show_Text("txt_error",log_error);
    switch(data.from){
        case "system":
            if (data.to=="30s:session"){
                session_id=data.message;
                sys.Show_Text("txt_session",session_id);  
                sys.Show_Text("txt_user_name",userName);
                sys.Send_Msg("sys_event",userName, "*_session", session_id); //服务器会记录用户名
            }
            break;
        case "progress1":
            var strSplit=data.message.split(":");
            sys.Show_ProgressBar("progress1","100",strSplit[0]);
            sys.Show_ProgressBar("progress2","100","100");
            break;
        case "progress2":
            var strSplit=data.message.split(":");
            sys.Show_ProgressBar('progress2', "100",strSplit[0]);
            break;
    }
}

function file_open(){
    var strLine=sys.File_Open();
    sys.Show_Text("txt_upload_file",strLine);
}



sys.Add_Text_Multi("txt_param","参数",10,10,300,400);

sys.Add_Text_Multi("txt_error","错误信息：",400,10,300,400);


sys.Add_Text("txt_user_name","000",10,450,100,30);
sys.Add_Text("txt_session","000",200,450,300,30);

sys.Add_Button("b1_1","批量运行",10,500,100,30,"run_click","");

sys.Add_Button("b1_2","停止",200,500,100,30,"stop_click","");

sys.Add_Progress("progress1",10,550,700,30);
sys.Add_Progress("progress2",10,600,700,30);

sys.Show_Form(900,700);

sys.Form_Title("Run_Bat");

sys.Connect_Socket("http://robot3.funnyai.com:7777","event_connected","event_chat","event_system");





