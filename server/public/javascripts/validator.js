/**
 * @Author: zhuangqh
 * @Email: zhuangqhc@gmail.com
 * @Create on: 2015/12/24
 */

var validator = {

  isFormatError: function (user) {
    if (user["username"] && user["password"]) {
      return this.isUsernameValid(user.username) && this.isPasswordValid(user.password);
    } else {
      return false;
    }
  },

  isUsernameValid: function (username){
    return /^[a-zA-Z][a-zA-Z0-9_]{5,18}$/.test(username);
  },

  isPasswordValid: function (password) {
    return /^[a-zA-Z][a-zA-Z0-9_\-]{5,12}$/.test(password);
  }
};

if (typeof module == 'object') {
  module.exports = validator;
}
