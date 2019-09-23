var disk="D:";
var userName="none";
var md5="";
var log_msg="";
var session_send=0;
var msg_id=0;
var myMap=[];
var head="";
var css_head='<html><head>\n'
+'<link href="http://www.funnyai.com/Common/css/default.css" type="text/css" rel="stylesheet" />\n'
+'<link href="http://www.funnyai.com/Common/css/table.css" type="text/css" rel="stylesheet" />\n'
+'<body>\n';
var sep=1;
var step=0;//处理步骤

[[[..\\data\\common_string.js]]]


//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}


function event_system(data){
    var obj=JSON.parse(data);
    var log_msg2=obj.from+"："+obj.message+"\r\n";
    s_ui.text_set("txt_info",log_msg2);
    switch(obj.type){
        case "chat_return":
            s_ui.text_set("txt_info","chat_return:"+obj.message);
            delete myMap["K"+obj.oid];
            break;
        case "30s:session":
            if (obj.from=="system"){
                session_id=obj.message;
                s_ui.text_set("txt_session",session_id);  
                s_ui.text_set("txt_user_name",userName);
                var friend="*";
                var strLine="{\"from\":\""+userName+"\",\"type\":\"session\",\"to\":\".\",\"message\":\""+session_id+"\"}";
                s_net.Send_Msg("sys_event",strLine); //服务器会记录用户名
                session_send=1;
            }
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            break;
    }
}



function event_chat(data){
    var obj=JSON.parse(data);
    var friend=obj.from;
    
 
    var index=s_ui.listbox_select("list_friend",friend);
    if (index==-1){
        friend_list("");
        s_ui.listbox_select("list_friend",friend);
    }
    var strMsg=s_time.Time_Now()+"<br>"+obj.message;
    s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",s_time.Date_Now()+" "+strMsg+"\r\n");
    
    var msg=obj.message;
    var r;
    switch(sep){
        case "file_list"://file
            //head="<tr><th>权限</th><th>子目录(文件)数</th><th>所属用户</th><th>所属用户组</th><th>大小</th><th colspan=3>最近修改/查看时间</th><th>名称</th></tr>";
            var strSplit=msg.split('\n');
            msg="";
            for (var i=0;i<strSplit.length;i++){
                r = /\s+/g;
                strSplit[i] = strSplit[i].replace(r, '|');
                var strSplit2=strSplit[i].split('|');
                
                msg+="<tr>";
                for (var j=0;j<strSplit2.length;j++){
                    var value=strSplit2[j];
                    if (j==8){
                        msg+="<td>"+value+"</td><td>"
                        +"<a href='http://file.edit?file="+value+"' target=_blank>编辑</a></td>";
                    }else{
                        msg+="<td>"+value+"</td>";
                    }
                }
                msg+="</tr>";
            }
            break;
        case 1:
            r = /\n/g;
            msg = msg.replace(r, '</td></tr><tr><td>');
            r = /\s+/g;
            msg = msg.replace(r, '</td><td>');
            break;
        case 2:
            r = /\n/g;
            msg = msg.replace(r, '</td></tr><tr><td>');
            r = /:/g;
            msg = msg.replace(r, '</td><td>');
            break;
    }
    msg=head+"\n"+msg;
    
    log_msg=s_time.Time_Now()+" "+friend+" &gt; <span style='color:#aaaaaa;'>"+obj.to+"</span>"
    +"<br><div><table id=data>"
    +msg+"</table></div><br><br>\r\n"+log_msg;
    
    var id=obj.id;
    var strLine="{\"from\":\""+userName+"\",\"type\":\"chat_return\",\"to\":\""+obj.from+"\",\"oid\":\""+id+"\",\"message\":\""+id+"\"}";
    s_net.Send_Msg("sys_event",strLine); //消息返回
    
    s_ui.Web_Content("web",css_head+log_msg);
    
    var line2="";
    if (step<3){
        var file=s_sys.value_read("file1");
        var line=s_file.read(file,1);
        var strSplit=line.split("|");
        var count=strSplit.length;
        
        for (var i=1;i<=count;i++){
            line2+="count(1)-sum(c"+i+"),";
        }
        if (line2.endsWith(",")){
            line2=line2.substr(0,line2.length-1);
        }
    }
    
    step+=1;
    var cmd="";
    switch(step){
        case 1:
            cmd="file_sql "+userName+" /root/step3.txt 250000 \"select "+line2+" from t;\" , /root/step3_2.txt";
            s_ui.text_set("txt_send",cmd);
            send_msg_click();
            break;
        case 2:
            cmd="cat /root/step3_2.txt";
            s_ui.text_set("txt_send",cmd);
            send_msg_click();
            break;
        case 3:
            break;
    }
}


function modify_click(data){
    var index=parseInt(data);
    var a=s_ui.datagrid_read("grid1",index,1);
    
    s_ui.Run_JS("Run_Bat\\step3_map.js");
    //s_ui.msg(a);
}

function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",3,"字段,类型,C");
    s_ui.datagrid_add_line("grid1","1,数字",",");
    s_ui.datagrid_add_line("grid1","2,字符串",",");
    s_ui.datagrid_add_line("grid1","3,数字",",");
    s_ui.datagrid_add_line("grid1","4,数字",",");
    s_ui.datagrid_add_line("grid1","5,字符串",",");
    s_ui.datagrid_add_line("grid1","6,数字",",");
    s_ui.datagrid_add_line("grid1","7,字符串",",");
    
    s_ui.datagrid_add_button("grid1","modify","映射","modify_click");
}

function static_click(data){
    var file=s_sys.value_read("file1");
    var line=s_file.read(file,1);
    var strSplit=line.split("|");
    var count=strSplit.length;
    
    var line="";
    for (var i=1;i<=count;i++){
        line+="isnumeric(c"+i+"),";
    }
    if (line.endsWith(",")){
        line=line.substr(0,line.length-1);
    }
    
    var file2=s_sys.value_read("file2");
    var cmd="file_sql "+userName+" /home/ftp_home"+file2+" 250000 \"select "+line+" from t;\" v /root/step3.txt";
    
    s_ui.text_set("txt_send",cmd);
    send_msg_click();
    step=0;
    
    //s_ui.msg(strSplit.length+"");
}


function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}


function read_ini(){
    var path=sys.AppPath();
    var strCount=s_file.Ini_Read(path+"\\config\\friend.ini","items","count");
    
    var userName2=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    userName=userName2+"/linux_bat_step3";
    
}

function friend_list(data){
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    s_net.Send_Msg("sys_event",strLine);
}


function event_connected(data){
    s_ui.text_set("txt_info","event_connected");
    s_ui.button_enable("btn_connect","0");
    friend_list();
    
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_ui.button_enable("btn_connect","1");
    s_net.Socket_Connect();
}

function connect_click(data){
    var url="http://robot6.funnyai.com:8000";
    s_net.Socket_Init(url,"event_connected","event_disconnected","event_chat","event_system");
    read_ini();
}


function select_old_friend(data){
    var friend=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","friend_selected");

    if (friend!=null && friend!=""){
        //if (s_ui.listbox_text("txt_info")==friend){
        var friend2=s_ui.listbox_text("list_friend");
        if (friend2!=friend){
        s_ui.text_set("txt_info","选择刚才选择的好友:"+friend);
        s_ui.listbox_select("list_friend",friend);
        }
    }
}

//检查是否联网
function check_connected(data){
    s_ui.text_set("txt_info","check_connected...");
    s_time.setTimeout("check_connected",2,"check_connected");
    
    if (s_net.Socket_Connected()){
        //friend_list("");
        
        if (session_send==1){
            select_old_friend("");
            //检查消息是否都发送过去了，没有发送的，再发送一次。
            resend_chat_msg("");
        }
    }else{
        session_send=0;
        s_net.Socket_Connect();//如果没有连，会自动连
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
        if (pMsg.Count<3){
            pMsg.Count+=1;
            s_net.Send_Msg("chat_event",pMsg.Msg);
        }else{
            var obj=JSON.parse(pMsg.Msg);
            var friend=pMsg.to;
            log_msg=s_time.Time_Now()+" <font color=red>(消息没有发送) </font> <span style='color:gray;'>"+obj.to+"</span><br>"
                    +obj.message+"<br><br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" 消息丢失："+obj.message+"\r\n");
                
            
            //s_ui.Web_Content("web",css_head+log_msg);
        }
    }
}


//发送消息 
function send_msg_click(){
    msg_id+=1;
    
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.msg("请选择好友！");
        return ;
    }
    //s_ui.combox_text("combox_head")+" "+
    var strMsg=s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");
    var strType="cmd";
    
    var token="";
    
    var url="http://www.funnyai.com/login_get_token_json.php";
    var name=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","account");
    var md5=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini","main","md5");
    var data="email="+s_string.urlencode(name)+"&password="+s_string.urlencode(md5);
    
    var result=s_net.http_post(url,data);
    if (result.indexOf("登录成功")>-1){
        var strSplit=result.split("=");
        token=strSplit[2];
    }


    var strLine="";
    
    var strMsg2=strMsg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    if (token==""){
        strLine="{\"id\":\""+msg_id+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
    }else{
        strLine="{\"id\":\""+msg_id+"\","
            +"\"token\":\""+token+"\","
            +"\"from\":\""+userName+"\",\"type\":\""+strType+"\","
            +"\"to\":\""+friend+"\",\"message\":\""+strMsg2+"\"}";
    }
    
    s_ui.text_set("txt_info",strLine);
    
    myMap["K"+msg_id]=new C_Msg(msg_id,strLine);
    
    s_net.Send_Msg("chat_event",strLine);
    
    
    log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
            +strMsg+"<br><br>"+log_msg;
    s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
        s_time.Date_Now()+" "+s_time.Time_Now()+" "+strMsg+"\r\n");
    
    s_ui.Web_Content("web",css_head+log_msg);
    s_ui.text_set("txt_send","");
    
}



s_ui.splitcontainer_init("split",0,0,500,500,"v");
s_ui.splitcontainer_distance("split",100);


s_ui.listbox_init("list_friend",10,60,200,180);
s_ui.listbox_init_event("list_friend","friend_change");

s_ui.text_init("txt_file",s_sys.value_read("file"),350,450,200,30);
//s_ui.button_init("b_clear","清空",250,30,450,30,"clear_click","");


//界面
s_ui.datagrid_init("grid1",10,60,650,220);

s_ui.text_init("txt_send","ls",380,350,320,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,90,30,"connect_click","");
s_ui.text_init("txt_session","000",10,500,200,30);



s_ui.textbox_init("txt_info","",10,250,200,80);


s_ui.splitcontainer_add("split",0,"list_friend","fill");
s_ui.splitcontainer_add("split",0,"txt_info","bottom");
s_ui.splitcontainer_add("split",0,"txt_user_name","bottom");
s_ui.splitcontainer_add("split",0,"btn_connect","bottom");
s_ui.splitcontainer_add("split",0,"txt_session","bottom");


s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");

s_ui.splitcontainer_add("split",1,"web","fill");

s_ui.splitcontainer_add("split",1,"grid1","top");
s_ui.splitcontainer_add("split",1,"txt_file","top");




s_ui.panel_init("panel_top",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel_top","bottom");
s_ui.panel_add("panel_top","txt_send","fill");

s_ui.panel_add("panel_top","b1_send","right");


s_ui.panel_init("panel2",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");


s_ui.button_init("b_static","重新统计分析",100,450,200,30,"static_click","");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat\\step2");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat\\step4");


s_ui.panel_add("panel2","b_static","left");
s_ui.panel_add("panel2","b_next","left");
s_ui.panel_add("panel2","b_pre","left");



s_ui.Menu_Init("Menu1",0,0,800,25);
s_ui.Menu_Add("Menu1","File","&File");
s_ui.Menu_Item_Add("Menu1","File","Refresh","刷新好友列表(&R)","friend_list","");

s_ui.Menu_Add("Menu1","Tools","&Tools");
s_ui.Menu_Item_Add("Menu1","Tools","Setting","设置(&S)","set_click","");



s_ui.acception_button("b1_send");
s_ui.Show_Form(800,600);
s_ui.Form_Title("第3步 字段映射");

data_init("");

connect_click("");
check_connected("");
