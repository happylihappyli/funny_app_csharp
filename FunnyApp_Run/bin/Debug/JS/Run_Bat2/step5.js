var friend_return=0;

var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击

var keep_count=1;

var myMap=[];
var head="";


[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]


var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");

function event_msg(data){
    var obj=JSON.parse(data);
    var msg=obj.message;

    switch(obj.type){
        case "chat_return":
            log_msg="<b>chat_return:"+obj.oid+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            s_ui.status_label_show("status_label2",msg);
            delete myMap["K"+obj.oid];
            break;
        case "login.ok":
            friend_list("");
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            friend_return=1;
            break;
        case "file_sql":
            if (obj.message=="finished"){
                
                log_msg="<b>finished "+obj.from+";"+step+"</b><br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
                switch (obj.from){
                    case "/root/step6.txt":
                        
                        log_msg=s_time.Time_Now()
                    +"<span style='color:red;font-size:18px;'>运行完毕！请点击下一步</span><br>"
                    +"<span style='color:blue;'>"+obj.from+"</span>"
                    +"<pre>"+msg+"</pre>"
                    +"<br>"+log_msg;
                        s_ui.Web_Content("web",css_head+log_msg);
                        break;
                }
            }
            break;
        case "msg":
            log_msg=s_time.Time_Now()
        +" <span style='color:blue;'>"+obj.from+"</span>"
        +obj.return_cmd+"<br>"
        +"<pre>"+msg+"</pre>"
        +"<br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+obj.from+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
            
            s_ui.Web_Content("web",css_head+log_msg);
            break;
        case "status":
            switch(obj.from){
                case "progress1":
                    var strSplit=msg.split(":");
                    s_ui.progress_show("progress1","100",strSplit[0]);
                    s_ui.progress_show("progress2","100","100");
                    break;
                case "progress2":
                    var strSplit=msg.split(":");
                    s_ui.progress_show('progress2', "100",strSplit[0]);
                    break;
            }
            break;
        default:
            log_msg=s_time.Time_Now()
                +"<span style='color:red;'>"+obj.from+"</span><br>"
                +msg+"<br><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            break;
    }
}



function sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}


function static_click(data){
    var file1=s_sys.value_read("file1");
    if (file1=="") file1="E:\\sample1.txt";
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    var count=strSplit.length;
    
    var line="";
    for (var i=1;i<=count;i++){
        var file_map=disk+"\\Net\\Web\\data\\map_c"+i+".txt";
        if (s_file.exists(file_map)){
            line+="map(c"+i+",'/home/ftp_home/upload/map/map_c"+i+".txt'),";
        }else{
            line+="c"+i+",";
        }
    }
    if (line.endsWith(",")){
        line=line.substr(0,line.length-1);
    }
    
    var file2=s_sys.value_read("file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    
    var cmd=sql("/home/ftp_home"+file2,
        "select "+line+" from t;","v","/root/step6.txt");
    step=1;
    s_ui.text_set("txt_send",cmd);
    send_msg_click();
}


function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}


function on_load(){
    userName=sys_read_ini()+"/linux_bat2";
    s_ui.text_set("txt_user_name",userName);
}

function friend_list(data){
    
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");

    send_msg("friend_list","","","friend_list");
}

function send_msg(strType,friend,msg,return_cmd){
    msg_id+=1;
    
    var token=sys_get_token();
    var strLine="";
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    
    if (token!=""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"return_cmd\":\""+return_cmd+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
        
        switch(strType){
            case "login":
            case "friend_list":
                break;
            default:
                myMap["K"+msg_id]=new C_Msg(msg_id,strLine);
                break;
        }
        
        log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                +msg+"<br><br>"
                +"<b>tcp.send:"+strLine+"</b>"
                +log_msg;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
            s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
        
        s_ui.Web_Content("web",css_head+log_msg);
    
    
        s_tcp.send("m:<s>:"+strLine+":</s>");
        
        
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
    
}


function event_connected(data){
    
    s_ui.status_label_show("status_label","event_connected!");
    s_ui.text_set("txt_info","event_connected");
    s_ui.button_enable("btn_connect","0");
    
    send_msg("login","","","login");
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_ui.button_enable("btn_connect","1");
    s_net.Socket_Connect();
}


function connect_click(data){
    
    s_tcp.connect("robot6.funnyai.com",6000,
        userName,"event_connected","event_msg");
    
    read_ini();
}


function select_old_friend(data){
    var friend=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","friend_selected");

    if (friend!=null && friend!=""){
        var friend2=s_ui.listbox_text("list_friend");
        if (friend2!=friend){
            s_ui.text_set("txt_info","选择刚才选择的好友:"+friend);
            s_ui.listbox_select("list_friend",friend);
            if (step==0){
                friend_return=0;
                static_click("");
            }
        }
    }
}

//检查是否联网
function check_connected(data){
    s_ui.text_set("txt_info","check_connected...");
    s_time.setTimeout("check_connected",2,"check_connected");
    
    if (friend_return==1){
        select_old_friend("");
        //检查消息是否都发送过去了，没有发送的，再发送一次。
        resend_chat_msg("");
    }
}


function friend_change(data){
    
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        //s_sys.value_save("friend_selected",friend);
        s_file.Ini_Save(disk+"\\Net\\Web\\main.ini","main","friend_selected",friend);
    }
}

function resend_chat_msg(data) {
    for(var key in myMap){
        var pMsg=myMap[key];
        pMsg.Count+=1;
        if (pMsg.Count<10){
            ;
        }else if (pMsg.Count==5){//再发送一次
            log_msg+="<b>resend:"+pMsg.Msg+"</b>";
            s_ui.Web_Content("web",css_head+log_msg);
            s_tcp.send("m:<s>:"+pMsg.Msg+":</s>");
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息发送失败) </font> <span style='color:gray;'>"
                    +obj.id+"="+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息发送失败："+obj.id+"="+obj.message+"\r\n");
            delete myMap["K"+obj.id];
        }
    }
}


//发送消息
function send_msg_click(){
    msg_id+=1;
    
    var strMsg=s_ui.text_read("txt_send");
    var strType="cmd";
    send_msg(strType,friend,strMsg,"step:"+step);
    s_ui.text_set("txt_send","");
}

function upload_click(data){
    
    var strLine=s_file.File_List_File(disk+"\\Net\\Web\\Data");
    var strSplit=strLine.split("|");

    for(var i=0;i<strSplit.length;i++){
        var file=disk+"\\Net\\Web\\Data\\"+strSplit[i];
        var path="/upload/map/"+s_file.File_Short_Name(file);
        //s_ui.msg(file+","+path);
        s_net.ftp_upload("robot6.funnyai.com","test","test","22",file,path,"set_status","show_error");
    }
    
    var file=s_ui.text_read("txt_upload");
    var path="/upload/map/"+s_file.File_Short_Name(file);

    var hosts=s_ui.text_read("txt_host");
    s_net.ftp_upload(hosts,"test","test","22",file,path,"set_status","show_error");
}

function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.text_set("txt_info",log_error);
}

s_ui.splitcontainer_init("split",0,0,500,500,"v");
s_ui.splitcontainer_distance("split",130);



var file1=s_sys.value_read("file1");
if (file1=="") file1="E:\\sample1.txt";
//s_ui.msg(file1);
s_ui.text_init("txt_file",file1,350,450,200,30);


//界面
//s_ui.datagrid_init("grid1",10,60,650,320);

s_ui.text_init("txt_send","ls",380,350,320,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.textbox_init("txt_info","",10,250,200,80);

s_ui.splitcontainer_add("split",0,"txt_info","fill");
s_ui.splitcontainer_add("split",0,"txt_user_name","bottom");


s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.progress_init("progress2",100,400,500,30);

s_ui.splitcontainer_add("split",1,"web","fill");

s_ui.splitcontainer_add("split",1,"progress2","top");

//s_ui.splitcontainer_add("split",1,"grid1","top");
s_ui.splitcontainer_add("split",1,"txt_file","top");



s_ui.panel_init("panel_top",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel_top","bottom");
s_ui.panel_add("panel_top","txt_send","fill");

s_ui.panel_add("panel_top","b1_send","right");


s_ui.panel_init("panel2",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step4");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step6");


s_ui.panel_add("panel2","b_next","left");
s_ui.panel_add("panel2","b_pre","left");



s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","Menu_File","&File");
s_ui.Menu_Item_Add("Menu1","Menu_File","Menu_Refresh","Friend_List","friend_list","");

s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Menu_Static","重新统计分析","static_click","");


s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");

s_ui.button_default("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("v2 第5步 生成step6.txt");

s_sys.tcp_event();

on_load("");

static_click("");


