
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");

var friend_return=0;
var session_send=0;
var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击
var fields_count=0;

var keep_count=1;

var myMap=[];
var head="";



function clear_data(data){
    s_ui.datagrid_clear("grid1");
}

function data_init(data){
    s_ui.datagrid_clear("grid1");
    
    s_ui.datagrid_init_column("grid1",9,"字段,备注,avg,方差,0%,25%,50%,75%,100%");
    s_ui.datagrid_add_line("grid1","1,正在分析...",",");
    
    s_ui.datagrid_add_checkbox("grid1","modify","选择","check_click");
    //s_ui.datagrid_add_button("grid1","modify","映射","map_click");
}


function check_click(data){
    var index=parseInt(data);
    var k=index+1;
    var a=s_ui.datagrid_read("grid1",index,8);
    
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    s_file.Ini_Save(file,"selected","check"+k,a);
}

function save_click(data){
    
    var index=parseInt(data)+1;
    var memo=s_ui.datagrid_read("grid1",index,1);
    memo=memo.replaceAll(",","");
    var file=disk+"\\Net\\Web\\Data\\memo.ini";
    s_file.Ini_Save(file,"main","memo"+index,memo);
    
    //s_ui.msg(memo);
}

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
        case "msg":
            log_msg=s_time.Time_Now()
                +" <span style='color:blue;'>"+obj.from+"</span>"
                +obj.return_cmd+"<br>"+"<pre>"+data+"</pre>"
                +"<br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
            if (obj.return_cmd=="step:1"){
                static_bad_click();
            }else if (obj.return_cmd=="step:2"){
                log_msg=s_time.Time_Now()
                    +" <span style='color:blue;'>"+obj.from+"</span>"
                    +"<pre style='color:red;'>运行完毕,点击下一步</pre>"
                    +"<br>"+log_msg;
                s_ui.Web_Content("web",css_head+log_msg);
            }
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
        
            log_msg="<b>data:"+data+"</b><br>"+log_msg;
            s_ui.Web_Content("web",css_head+log_msg);
    
            break;
    }
}



function friend_list(data){
    s_ui.listbox_clear("list_friend");
    s_ui.listbox_add("list_friend","*");

    send_msg("friend_list","","","friend_list");
    
}

function friend_change(data){
    
    var friend=s_ui.listbox_text("list_friend");
    if (friend!=""){
        s_file.Ini_Save(disk+"\\Net\\Web\\main.ini","main","friend_selected",friend);
    }
}



function sql(file1,sql,sep,output){
    var cmd="file_sql /root/happyli/set_hadoop.ini "+userName+" "+file1
        +" 250000 \""+sql+"\" "+sep+" "+output;
    return cmd;
}


function map_click(data){
    var index=parseInt(data);
    row_index=s_ui.datagrid_read("grid1",index,0);
    s_sys.value_save("row_index",row_index);
    
    var type=s_ui.datagrid_read("grid1",index,1);
    
    
}

function get_compare(index){
    switch(index){
        case "0":
            return ">";
        case "1":
            return ">=";
        case "2":
            return "=";
        case "3":
            return "<";
        case "4":
            return "<=";
        case "5":
            return "<>";
    }
    return "";
}

function static_click(data){
    var file1=s_file.Ini_Read(file_memo,"main","file1");
    if (file1=="") file1="E:\\sample1.txt";
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    fields_count=strSplit.length;
    
    
    var file2=s_file.Ini_Read(file_memo,"main","file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    var line="";
    
    var file_ini=disk+"\\Net\\Web\\Data\\memo.ini";

    for (var i=1;i<=fields_count;i++){
        
        var value=s_file.Ini_Read(file_ini,"selected","check"+i);
        if (value=="1"){
            var file_map=disk+"\\Net\\Web\\data\\map_c"+i+".txt";
            if (s_file.exists(file_map)){
                line+="map(c"+i+",'/home/ftp_home/upload/map/map_c"+i+".txt'),";
            }else{
                line+="c"+i+",";
            }
        }
    }
    if (line.endsWith(",")){
        line=line.substr(0,line.length-1);
    }
    
    var strWhere="";
    var y_index=parseInt(s_file.Ini_Read(file_ini,"y","index"));
    var c1=s_file.Ini_Read(file_ini,"good","compare");
    var good=s_file.Ini_Read(file_ini,"good","value");
    var good_map=s_file.Ini_Read(file_ini,"good","map");
    var compare=get_compare(c1);
    strWhere="c"+y_index+compare+good;
    
    
    var cmd=sql("/home/ftp_home"+file2,
        "select "+line+" from t where "+strWhere+";",
        "v","/root/step8_good.txt");
    
    step=1;
    send_msg("cmd",friend,cmd,"step:"+step);
    
}

function static_bad_click(){
    var file1=s_file.Ini_Read(file_memo,"main","file1");
    if (file1=="") file1="E:\\sample1.txt";
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    fields_count=strSplit.length;
    
    
    var file2=s_file.Ini_Read(file_memo,"main","file2");
    if (file2=="") file2="/upload/sample1.txt";
    
    var line="";
    
    var file_ini=disk+"\\Net\\Web\\Data\\memo.ini";

    for (var i=1;i<=fields_count;i++){
        
        var value=s_file.Ini_Read(file_ini,"selected","check"+i);
        if (value=="1"){
            var file_map=disk+"\\Net\\Web\\data\\map_c"+i+".txt";
            if (s_file.exists(file_map)){
                line+="map(c"+i+",'/home/ftp_home/upload/map/map_c"+i+".txt'),";
            }else{
                line+="c"+i+",";
            }
        }
    }
    if (line.endsWith(",")){
        line=line.substr(0,line.length-1);
    }
    
    var strWhere="";
    var y_index=parseInt(s_file.Ini_Read(file_ini,"y","index"));
    var c1=s_file.Ini_Read(file_ini,"bad","compare");
    var bad=s_file.Ini_Read(file_ini,"bad","value");
    var bad_map=s_file.Ini_Read(file_ini,"bad","map");
    var compare=get_compare(c1);
    strWhere="c"+y_index+compare+bad;
    
    
    var cmd=sql("/home/ftp_home"+file2,
        "select "+line+" from t where "+strWhere+";",
        "v","/root/step8_bad.txt");
    
    step=2;
    send_msg("cmd",friend,cmd,"step:"+step);
}

function next_click(data){
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

function on_load(){
    var a=sys_read_ini();
    userName=a+"/linux_bat2";
    s_ui.text_set("txt_user_name",userName);
    
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


function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.text_set("txt_info",log_error);
}


function send_msg(strType,friend,msg,return_cmd){
    msg_id+=1;
    
    var token=sys_get_token();
    var strLine="";
    
    var strMsg2=msg.replaceAll("\"","\\\"");
    strMsg2=strMsg2.replaceAll("\n","\\n");
    
    //s_ui.msg(token);
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
        
        s_tcp.send("m:<s>:"+strLine+":</s>");
        
        
        log_msg=s_time.Time_Now()+" 我 &gt; <span style='color:gray;'>"+friend+"</span><br>"
                +msg+"<br><br>"+"<b>tcp.send:"+strLine+"</b>"
                +log_msg;
        s_file.append(disk+"\\Net\\Web\\log\\"+friend+".txt",
            s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
        
        s_ui.Web_Content("web",css_head+log_msg);
    }else{
        s_ui.status_label_show("status_label","token==null!");
    }
}


s_ui.splitcontainer_init("split",0,0,500,500,"h");
s_ui.splitcontainer_distance("split",30);


s_ui.text_init("txt_file",s_sys.value_read("file"),350,450,200,30);


//界面


s_ui.textbox_init("txt_user_name","000",10,450,200,30);


s_ui.textbox_init("txt_info","",10,250,300,50);


s_ui.panel_init("panel_top1",0,0,500,25,"none");

s_ui.splitcontainer_add("split",0,"panel_top1","fill");

s_ui.panel_add("panel_top1","txt_info","fill");
s_ui.panel_add("panel_top1","txt_user_name","right");


s_ui.Web_Init("web",250,60,450,250);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.progress_init("progress2",100,400,500,30);

s_ui.splitcontainer_add("split",1,"web","fill");

s_ui.splitcontainer_add("split",1,"progress2","top");



s_ui.panel_init("panel_top",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel_top","bottom");


s_ui.panel_init("panel2",0,0,500,50,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step7");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step9");


s_ui.panel_add("panel2","b_next","left");
s_ui.panel_add("panel2","b_pre","left");



s_ui.menu_init("Menu1");

s_ui.menu_add("Menu1","Tools","&Tools");
s_ui.menu_item_add("Menu1","Tools","Menu_Static","重新统计分析","static_click","");


s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");


s_ui.button_default("b1_send");
s_ui.show_form(800,600);
s_ui.Form_Title("v2 第8步 好坏样本分离");


s_sys.tcp_event();

on_load("");

data_init("");
static_click("");
