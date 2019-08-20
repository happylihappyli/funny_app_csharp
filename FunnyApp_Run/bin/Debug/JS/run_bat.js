
var index=0;
var userName="test";

var log_output="";
var log_error="";

//按钮点击

function run_click(data){
    index=0;
    run_next();
}

function stop_click(data){
    
}

function run_next(){
    var count=sys.ListBox_Item_Size("list_param");
    if (index<count){
        sys.Show_ProgressBar("progress0",count+"",index+"");
        sys.ListBox_Item_Selected("list_param",index);
        var strLine=sys.ListBox_Item("list_param",index);
        var strSplit=strLine.split(",");
        var id=strSplit[0];
        var param=strSplit[1];
        var url="http://www.funnyai.com/funnyscript/fs_run_line_shell.php?id="+id+"&user="
    +encodeURIComponent(userName)+"&param="+encodeURIComponent(param);
        show_error(url);
        var result=sys.Net_Http_GET(url);
        show_error(result);
        index+=1;
    }else{
        show_error("运行完毕");
        sys.Show_ProgressBar("progress0","100","100");
    }
}

function set_status(data){
    var strSplit=data.split(",");
    sys.Show_ProgressBar("progress1",strSplit[1],strSplit[0]);
}

function show_error(data){
    log_error=data+"\r\n"+log_error;
    sys.Show_Text("txt_error",log_error);
}


function send_msg_click(){
    sys.Send_Msg("chat_event","csharp","*",sys.Get_Text("send_msg"));
}

var log_error="";
function event_connected(data){
    log_error="Connected\r\n"+log_error;
    sys.Show_Text("txt_error",log_error);
}


function event_chat(data1){
    var data=JSON.parse(data1);
    log_output=data.message+"\r\n"+log_output;
    sys.Show_Text("txt_output",log_output);
    
}

function event_system(data1){
    //sys.Msg(data1);
    var data=JSON.parse(data1);
    //sys.Msg(data.type);
    switch(data.type){
        case "30s:session":
            session_id=data.message;
            sys.Show_Text("txt_session",session_id);  
            sys.Show_Text("txt_user_name",userName);
            var line="{\"from\":\""+userName+"\","
                +"\"type\":\"session\",\"to\":\"\",\"message\":\""+session_id+"\"}";
            sys.Send_Msg("sys_event",line); //服务器会记录用户名
            break;
        default:
            switch(data.from){
                case "C_Command":
                    log_error="sys:"+data.from+"="+data.message+"\r\n"+log_error;
                    sys.Show_Text("txt_error",log_error);
                    //output('<span class="username-msg">' + data.from + ':</span> ' + data.message);
                    run_next();
                    break;
                case "C_Command.Output":
                    log_error="sys:"+data.from+"="+data.message+"\r\n"+log_error;
                    sys.Show_Text("txt_error",log_error);
                    //$("#result").html( converter.makeHtml(data.message)+"<hr>"+$("#result").html());
                    break;
                case "C_Command.Error":
                    log_error="sys:"+data.from+"="+data.message+"\r\n"+log_error;
                    sys.Show_Text("txt_error",log_error);
                    //$("#error").html( converter.makeHtml(data.message) +"<hr>"+$("#error").html());
                    break;

                case "system":
                    log_error="sys:"+data.from+"="+data.message+"\r\n"+log_error;
                    sys.Show_Text("txt_error",log_error);
                    
                    break;
                case "progress1":
                    var strSplit=data.message.split(":");
                    sys.Show_ProgressBar("progress1","100",strSplit[0]);
                    sys.Show_ProgressBar("progress2","100","100");
                    break;
                case "progress2":
                    //log_error="progress2\r\n"+log_error;
                    //sys.Show_Text("txt_error",log_error);
                    var strSplit=data.message.split(":");
                    sys.Show_ProgressBar('progress2', "100",strSplit[0]);
                    break;
            }
            break;
    }
}

function file_open(){
    var strLine=sys.File_Open();
    sys.Show_Text("txt_upload_file",strLine);
}

function add_bat(data){
    var text=sys.Get_Text("txt_param");
    sys.ListBox_Add_Bat("list_param",text);
}

function param_click(data){
    sys.Run_JS("run_bat_param.js");
}
sys.Add_Text_Multi("txt_param","参数",350,10,600,150);
sys.Add_ListBox("list_param","参数",10,10,300,400);
sys.Add_Text_Multi("txt_error","错误信息：",350,200,300,210);
sys.Add_Text_Multi("txt_output","output：",660,200,300,210);

sys.Add_Button("b1_1","生成list",10,450,100,30,"add_bat","");

sys.Add_Text("txt_user_name","000",150,450,100,30);
sys.Add_Text("txt_session","000",350,450,300,30);

sys.Add_Button("b2_1","批量运行",10,500,100,30,"run_click","");

sys.Add_Button("b2_2","停止",150,500,100,30,"stop_click","");

sys.Add_Button("b2_3","参数",250,500,100,30,"param_click","");

sys.Add_Progress("progress0",10,550,700,30);
sys.Add_Progress("progress1",10,600,700,30);
sys.Add_Progress("progress2",10,650,700,30);

sys.Show_Form(1000,800);

sys.Form_Title("Run_Bat");

var url="http://robot6.funnyai.com:8000";
sys.Connect_Socket(url,"event_connected","event_chat","event_system");





