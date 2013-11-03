<?php
Include_once ('config.php');
class MyBase
{
    private $mMySqli;
	
    function  __construct()
    {
    $this->mMySqli = new mysqli(DB_HOST,DB_USER,DB_PASSWORD,DB_DATABASE);
    $this->mMySqli->query("SET names 'cp1251'");
    $this->mMySqli->query("SET collation_connection='cp1251_general_ci'");
    $this->mMySqli->query("SET collation_server='cp1251_general_ci'");
    $this->mMySqli->query("SET character_set_client='cp1251'");
    $this->mMySqli->query("SET character_set_connection='cp1251'");
    $this->mMySqli->query("SET character_set_results='cp1251'");
    $this->mMySqli->query("SET character_set_server='cp1251'");
    }

    function  __destruct() {
        $this->mMySqli->close();
    }
	public function GenSID()
    {
      $rows=$this->mMySqli->query('Select UUID()');
      $SID = $rows->fetch_array(MYSQLI_ASSOC);
      $rows->Close();
      return $SID['UUID()'];
    }
	public function mmail($mail)
	{
		if($result2 = $this->mMySqli->query('SELECT COUNT(*) AS Summ FROM  pubkeys WHERE email="'.$mail.'"')){
        	$row = $result2->fetch_array(MYSQLI_ASSOC);
        	$count = $row['Summ']; 
        	$result2->Close();
			}
		if($count>=1){return false;}
		return true;
	}
	
	public function regUser($SID,$mail)
	{
		$this->mMySqli->query("INSERT INTO pubkeys (email,pubkey,action) VALUES ('".$mail."','".$SID."',0)");
	}
	
	public function getKey($mail){	
	$count='not';
	if ($result = $this->mMySqli->query('SELECT pubkey FROM pubkeys where email="'.$mail.'" and action=1')){
        $row = $result->fetch_array(MYSQLI_ASSOC);
        $count = $row['pubkey']; 
        $result->Close();
        return $count;}else{return "";}
	}

	public function actionUser($SID,$mail)
	{
		$this->mMySqli->query("UPDATE pubkeys SET action=1, pubkey='' where email='".$mail."' and action=0 and pubkey='".$SID."'");
	}

	public function UpdatePubKey($SID,$mail)
	{
		$this->mMySqli->query("UPDATE pubkeys SET pubkey='".$SID."' where email='".$mail."' and action=1");
	}

	
	
}

 global $MyBaseReg;
 $MyBaseReg = new MyBase(); 
 
?>
