
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");


var msg_id=0;
var sep=1;
var step=0;//处理步骤
var row_index=0;//第几个字段被点击

var log_msg="";
var keep_count=1;

var myMap=[];
var head="";


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
            s_ui.status_label_show("status_label2","login.ok");
            friend_list("");
            break;
        case "list.all":
            s_ui.listbox_add("list_friend",obj.message);
            friend_return=1;
            break;
        case "status":
            break;
        case "msg":
            log_msg=s_time.Time_Now()
        +" <span style='color:blue;'>"+obj.from+"</span>"
        +obj.type+":"
        +obj.return_cmd+"<br>"
        +"<pre>"+msg+"</pre>"
        +"<br>"+log_msg;
            s_file.append(disk+"\\Net\\Web\\log\\"+obj.from+".txt",
                s_time.Date_Now()+" "+s_time.Time_Now()+" "+msg+"\r\n");
            
            s_ui.Web_Content("web",css_head+log_msg);
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
    
    s_ui.status_label_show("status_label","login!");
    send_msg("login","","","login");
}

function event_disconnected(data){
    s_ui.text_set("txt_info","event_disconnected");
    s_ui.button_enable("btn_connect","1");
    s_net.Socket_Connect();
}

function event_tcp_error(data){
    s_ui.msg(data);
}

function connect_click(data){
    
    s_tcp.connect("robot6.funnyai.com",6000,
        userName,"event_connected","event_msg","event_tcp_error");
    
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
                //static_click("");
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
    
    var index=s_ui.listbox_index("list_friend");
    if (index<0){
        s_ui.status_label_show("status_label","请选择好友！!");
        //s_ui.msg("请选择好友！");
        return ;
    }
    var strMsg=s_ui.text_read("txt_send");
    var friend=s_ui.listbox_text("list_friend");
    var strType="cmd";
    send_msg(strType,friend,strMsg,"step:"+step);
    s_ui.text_set("txt_send","");
}


function set_click(data){
    s_ui.Run_JS("Run_Bat2\\setting_user.js");
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


function show_tools(data){
    s_ui.Run_JS(data+".js");
}

function show_train(data){
    
    var alg=s_ui.combox_text("cb_alg");
    switch(alg){
        case "GBDT":
            s_ui.Run_JS("Run_Bat2\\step_train_gbdt.js");
            break;
        case "随机森林":
            s_ui.Run_JS("Run_Bat2\\step_train_rf.js");
            break;
        case "逻辑回归":
            s_ui.Run_JS("Run_Bat2\\step_train_lr.js");
            break;
        case "KNN":
            
            s_ui.Run_JS("Run_Bat2\\step_train_knn.js");
            break;
    }
    
}


function show_test(data){
    
    var alg=s_ui.combox_text("cb_alg");
    switch(alg){
        case "GBDT":
            s_ui.Run_JS("Run_Bat2\\step_test_gbdt.js");
            break;
        case "随机森林":
            s_ui.Run_JS("Run_Bat2\\step_test_rf.js");
            break;
        case "逻辑回归":
            s_ui.Run_JS("Run_Bat2\\step_test_lr.js");
            break;
        case "KNN":
            
            s_ui.Run_JS("Run_Bat2\\step_test_knn.js");
            break;
    }
    
}


function show_tools2(data){
    s_ui.Run_JS_Out(data+".js");
}

function clear_click(data){
    log_msg="";
    s_ui.Web_Content("web",css_head+log_msg);
}

s_ui.splitcontainer_init("split",0,0,500,500,"v");
s_ui.splitcontainer_distance("split",130);


s_ui.listbox_init("list_friend",10,60,200,180);
s_ui.listbox_init_event("list_friend","friend_change");



var file1=s_file.Ini_Read(file_memo,"main","file1");
if (file1=="") file1="E:\\sample1.txt";
//s_ui.msg(file1);
//s_ui.text_init("txt_file",file1,350,450,200,30);


//界面
s_ui.text_init("txt_send","ls",380,350,320,30);


s_ui.button_init("b1_send","发送",600,400,100,30,"send_msg_click","");



s_ui.text_init("txt_user_name","000",10,450,100,30);
s_ui.button_init("btn_connect","连服务器",120,450,90,30,"connect_click","");

s_ui.textbox_init("txt_info","",10,250,200,80);


s_ui.splitcontainer_add("split",0,"list_friend","fill");
s_ui.splitcontainer_add("split",0,"txt_info","bottom");
s_ui.splitcontainer_add("split",0,"txt_user_name","bottom");
s_ui.splitcontainer_add("split",0,"btn_connect","bottom");


s_ui.Web_Init("web",250,60,450,150);
s_ui.Web_Content("web","接收到信息");
s_ui.Web_New_Event("web","New_URL");


s_ui.panel_init("panel_main0",0,0,500,50,"none");
s_ui.panel_init("panel_main1",0,0,500,50,"none");
s_ui.panel_init("panel_main2",0,0,500,50,"none");
s_ui.panel_init("panel_main3",0,0,500,50,"none");
s_ui.panel_init("panel_main4",0,0,500,50,"none");
s_ui.panel_init("panel_main5",0,0,500,50,"none");


s_ui.splitcontainer_add("split",1,"web","fill");

s_ui.splitcontainer_add("split",1,"panel_main5","top");
s_ui.splitcontainer_add("split",1,"panel_main4","top");
s_ui.splitcontainer_add("split",1,"panel_main3","top");
s_ui.splitcontainer_add("split",1,"panel_main2","top");
s_ui.splitcontainer_add("split",1,"panel_main1","top");
s_ui.splitcontainer_add("split",1,"panel_main0","top");

s_ui.panel_init("panel_top",0,0,500,25,"none");
s_ui.panel_init("panel_bottom",0,0,500,25,"none");


///////////////////////////////////////////
s_ui.label_init("lb_alg","算法选择:",100,320);
s_ui.combox_init("cb_alg","逻辑回归",200,320,160,30);
s_ui.combox_add("cb_alg","逻辑回归");
s_ui.combox_add("cb_alg","GBDT");
s_ui.combox_add("cb_alg","随机森林");
s_ui.combox_add("cb_alg","KNN");

s_ui.panel_add("panel_top","lb_alg","right");
s_ui.panel_add("panel_top","cb_alg","right");

///////////////////////////////////////////
s_ui.splitcontainer_add("split",1,"panel_top","top");
s_ui.splitcontainer_add("split",1,"panel_bottom","bottom");
s_ui.panel_add("panel_bottom","txt_send","fill");
s_ui.panel_add("panel_bottom","b1_send","right");

s_ui.panel_init("panel2",0,0,500,25,"none");
s_ui.splitcontainer_add("split",1,"panel2","bottom");

///////////////////////////////////////////
s_ui.button_init("b_model_step31","相关性",10,150,100,30,"show_tools","Run_Bat2\\step_correlation");


///////////////////////////////////////////
s_ui.button_init("b_model_step_split","分隔符转化",10,100,100,30,"show_tools","Run_Bat2\\step_split");
s_ui.button_init("b_model_step_head","文件头处理",10,100,100,30,"show_tools","Run_Bat2\\step_head");
s_ui.panel_add("panel_main0","b_model_step_split","right");
s_ui.panel_add("panel_main0","b_model_step_head","right");


///////////////////////////////////////////
s_ui.button_init("b_model_step1","1-文件上传",10,100,100,30,"show_tools","Run_Bat2\\step1");
s_ui.button_init("b_model_step2","2-选择文件",10,100,100,30,"show_tools","Run_Bat2\\step2");
s_ui.button_init("b_model_step3","3-字段映射",10,100,100,30,"show_tools","Run_Bat2\\step3");
s_ui.button_init("b_model_step4","4-上传映射",10,100,100,30,"show_tools","Run_Bat2\\step4");
s_ui.button_init("b_model_step5","5-处理文件",10,100,100,30,"show_tools","Run_Bat2\\step5");
s_ui.panel_add("panel_main1","b_model_step1","right");
s_ui.panel_add("panel_main1","b_model_step2","right");
s_ui.panel_add("panel_main1","b_model_step3","right");
s_ui.panel_add("panel_main1","b_model_step4","right");
s_ui.panel_add("panel_main1","b_model_step5","right");


///////////////////////////////////////////
s_ui.button_init("b_model_step5_5","WOE IV",10,150,100,30,"show_tools","Run_Bat2\\step_woe_iv");
s_ui.button_init("b_model_step6","6-选择字段",10,150,100,30,"show_tools","Run_Bat2\\step6");
s_ui.button_init("b_model_step7","7-好坏样本",10,150,100,30,"show_tools","Run_Bat2\\step7");
s_ui.button_init("b_model_step8","8-数据分离",10,150,100,30,"show_tools","Run_Bat2\\step8");
s_ui.button_init("b_model_step9","9-数据抽样",10,150,100,30,"show_tools","Run_Bat2\\step9");
s_ui.panel_add("panel_main2","b_model_step5_5","right");
s_ui.panel_add("panel_main2","b_model_step6","right");
s_ui.panel_add("panel_main2","b_model_step7","right");
s_ui.panel_add("panel_main2","b_model_step8","right");
s_ui.panel_add("panel_main2","b_model_step9","right");


///////////////////////////////////////////
s_ui.button_init("b_model_step_train","模型训练",10,150,100,30,"show_train","");
s_ui.panel_add("panel_main3","b_model_step_train","right");



///////////////////////////////////////////
s_ui.button_init("b_model_step21","测试样本",10,150,100,30,"show_tools","Run_Bat2\\step_test_data");
s_ui.button_init("b_model_step22","模型测试",10,150,100,30,"show_test","");
s_ui.button_init("b_model_step23","统计准备",10,150,100,30,"show_tools","Run_Bat2\\step_static_pre");
s_ui.button_init("b_model_step24","KS计算",10,150,100,30,"show_tools","Run_Bat2\\step_ks");
s_ui.button_init("b_model_step25","ROC",10,150,100,30,"show_tools","Run_Bat2\\step_roc");
s_ui.panel_add("panel_main4","b_model_step21","right");
s_ui.panel_add("panel_main4","b_model_step22","right");
s_ui.panel_add("panel_main4","b_model_step23","right");
s_ui.panel_add("panel_main4","b_model_step24","right");
s_ui.panel_add("panel_main4","b_model_step25","right");


///////////////////////////////////////////
s_ui.panel_add("panel_main5","b_model_step31","right");



///////////////////////////////////////////
s_ui.menu_init("Menu1");
s_ui.menu_add("Menu1","Menu_File","&File");
s_ui.menu_item_add("Menu1","Menu_File","Menu_Refresh","Friend_List","friend_list","");

s_ui.menu_add("Menu1","Tools","&Tools");
s_ui.menu_item_add("Menu1","Tools","Menu_Clear","清空记录","clear_click","");
s_ui.menu_item_add("Menu1","Tools","Menu_Setting","设置","set_click","");


///////////////////////////////////////////

s_ui.status_init("status",0,0,200,30,"bottom");
s_ui.status_label_init("status_label","111",100,30);
s_ui.status_add("status","status_label","left");
s_ui.status_label_init("status_label2","222",100,30);
s_ui.status_add("status","status_label2","left");

s_ui.button_default("b1_send");
s_ui.show_form(800,600);
s_ui.Form_Title("分析工具 v2");

s_sys.tcp_event();

on_load("");

connect_click("");

check_connected("");


