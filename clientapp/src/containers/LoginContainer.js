﻿import LoginView from "../views/Login";
import {useDispatch} from "react-redux";
import {useFormik} from "formik";
import * as Yup from "yup";
import {loginRequest} from "../redux/actions";

export const LoginContainer = (props) => {
    const dispatch = useDispatch();
    const formik = useFormik({
        initialValues: {
            email: '',
            password: ''
        },
        validationSchema: Yup.object({
            email: Yup.string().email('Invalid email')
                .max(30, 'Must be less than 30 characters')
                .required('Required'),
            password: Yup
                .string()
                .min(6, 'Must be more than 6 characters')
                .max(50, 'Must be less than 50 characters')
                .required('Required')
        }),
        onSubmit: (values) => {
            dispatch(loginRequest(values));
        }
    });

    return <LoginView
        onSubmitForm={formik.handleSubmit}
        formik={formik}
    />
};