import {call, put} from "redux-saga/effects";
import {axiosDefault, axiosWithJwt} from "../../axios";
import {
    loginRequestFailed,
    loginRequestSuccess, logoutRequestFailed,
    logoutRequestSuccess,
    registerRequestFailed,
    registerRequestSuccess
} from "../actions";

export function* sagaLoginRequest(data){
    try {
        const response = yield call(
            axiosDefault,
            'https://localhost:5001/login',
            'post',
            JSON.stringify(data.payload)
        );

        if(response.data.jwtToken)
            yield put(loginRequestSuccess(response.data));
        else
            yield put(loginRequestFailed('Login failed. There is not jwt token'));
    } catch (e) {
        yield put(loginRequestFailed(e.response.data));
    }
}

export function* sagaRegisterRequest(data){
    try {
        const response = yield call(
            axiosDefault,
            'https://localhost:5001/register',
            'post',
            JSON.stringify(data.payload)
        );

        if(response.status === 200)
            yield put(registerRequestSuccess(response.data));
        else
            yield put(registerRequestFailed('Register failed'));
    } catch (e) {
        yield put(registerRequestFailed(e.response.data));
    }
}

export function* sagaLogoutRequest(data){
    try{
        const response = yield call(
            axiosWithJwt,
            'https://localhost:5001/logout',
            'get',
            data.payload
        )

        if(response.status === 200)
            yield put(logoutRequestSuccess());
        else
            yield put(logoutRequestFailed('Logout failed'));
    } catch (e) {
        yield put(logoutRequestFailed(e.response.data));
    }
}