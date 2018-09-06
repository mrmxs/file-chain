class Form {
    constructor() {
        this._name = '';
        this._surname = '';
        this._login = '';
        this._password = '';
        this._XHR = false;

        this._api = 'http://ec2-18-220-242-48.us-east-2.compute.amazonaws.com/api';
    }
    
    init() {
        this._$login = $('.js-l-login');
        this._$password = $('.js-l-password');
        this._$signupLogin = $('.js-s-login');
        this._$signupPassword = $('.js-s-password');
        this._$name = $('.js-name');
        this._$surname = $('.js-surname');

        this._$toggleFormButton = $('.js-toggle-form-btn');
        this._$signinButton = $('.js-signin-btn');
        this._$loginButton = $('.js-login-btn');
        this._$loginForm = $('.js-form-login');
        this._$signinForm = $('.js-form-signin');

        this._$toggleFormButton.on('click', this._onClickToggleFormButton.bind(this));
        this._$login.on('change', this._onChangeLogin.bind(this));
        this._$signupLogin.on('change', this._onChangeLogin.bind(this));
        this._$password.on('change', this._onChangePassword.bind(this));
        this._$signupPassword.on('change', this._onChangePassword.bind(this));
        this._$signinButton.on('click', this._onClickSignin.bind(this));
        this._$loginButton.on('click', this._onClickLogin.bind(this));
    }

    get name() {
        return this._$name.val();
    }

    get surname() {
        return this._$surname.val();
    }

    _onClickToggleFormButton() {
        if (this._XHR) {
            return;
        }

        this._$loginForm.toggleClass('form_active');
        this._$signinForm.toggleClass('form_active');

        this._$login.val(this._login);
        this._$password.val(this._password);
        this._$signupLogin.val(this._login);
        this._$signupPassword.val(this._password);
    }

    _onChangeLogin(event) {
        this._login = event.target.value;
    }

    _onChangePassword(event) {
        this._password = event.target.value;
    }

    _onClickLogin() {
        if (this._XHR) {
            return;
        }

        const sentData = {
            login: this._login,
            password: this._password
        };

        $.ajax({
            type: 'DELETE',
            headers: {
                'Access-Control-Allow-Origin': '*'
            },
            data: sentData,
            url: `${this._api}/user`,
            dataType: 'json',
            beforeSend: () => {
                this._XHR = true;
            },
            success: response => {
                console.warn(response);
            },
            error: error => {
                console.error(error);
            },
            complete: () => {
                console.warn(this._XHR);
                this._XHR = false;
            }
        });
    }

    _onClickSignin() {
        if (this._XHR) {
            return;
        }

        const sentData = {
            login: this._login,
            password: this._password,
            firstname: this.name,
            lastname: this.surname
        };

        $.ajax({
            type: 'POST',
            headers: {
                'Access-Control-Allow-Origin': '*'
            },
            data: sentData,
            url: `${this._api}/user`,
            dataType: 'json',
            beforeSend: () => {
                this._XHR = true;
            },
            success: response => {
                console.warn(response);
            },
            error: error => {
                console.error(error);
            },
            complete: () => {
                console.warn(this._XHR);
                this._XHR = false;
            }
        });
    }
};

document.addEventListener('DOMContentLoaded', () => {
    (new Form()).init();
});
