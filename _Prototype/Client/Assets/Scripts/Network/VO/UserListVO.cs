using System.Collections.Generic;
[System.Serializable]
public class UserListVO : VO
{
    public List<UserVO> dataList;

    public UserListVO()
    {

    }

    public UserListVO(List<UserVO> list)
    {
        dataList = list;
    }
}
