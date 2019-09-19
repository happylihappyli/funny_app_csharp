
data=s_sys.Value_Read("sleep");
var isec=parseInt(data);
if (isec>0){
    s_sys.sleep(isec);
}