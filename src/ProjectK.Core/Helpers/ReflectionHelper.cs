﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace ProjectK.Core.Helpers
{
    public class ReflectionHelper
    {
        public object GetPropertyValue(object o, string member)
        {
            // check inputs
            if (o == null) throw new ArgumentNullException(nameof(o));
            if (member == null) throw new ArgumentNullException(nameof(member));

            // get the type of the object
            var scope = o.GetType();

            // convert to metadata provider
            var provider = o as IDynamicMetaObjectProvider;

            // if object is a provider
            if (provider != null)
            {
                // check object contains requested property
                if (!((IDictionary<string, object>)provider).ContainsKey(member))
                {
                    throw new ArgumentException($"'{member}' could not be found in this object.");
                }

                // get the parameter
                var param = Expression.Parameter(typeof(object));

                // get the meta data object
                var mobj = provider.GetMetaObject(param);

                // get the binder
                var binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, member, scope, new[] { CSharpArgumentInfo.Create(0, null) });

                // bind the member
                var ret = mobj.BindGetMember(binder);

                // create expression
                var final = Expression.Block(
                    Expression.Label(CallSiteBinder.UpdateLabel),
                    ret.Expression
                );

                // create lambda
                var lambda = Expression.Lambda(final, param);

                // compile lambda
                var del = lambda.Compile();

                // invoke lambda and return
                return del.DynamicInvoke(o);
            }

            // get member type
            var memberObj = o.GetType().GetProperty(member, BindingFlags.Public | BindingFlags.Instance);

            if (memberObj == null)
            {
                throw new ArgumentException($"'{member}' could not be found in this object.");
            }

            // get value from member
            var value = memberObj.GetValue(o, null);

            // check if value is null
            if (value != null)
            {
                // get the underlying type
                var underlyingType = memberObj.PropertyType;

                // check if property is nullable
                var isNullable = IsNullableType(underlyingType);

                // if its nullable
                if (isNullable)
                {
                    // get underlying top
                    underlyingType = Nullable.GetUnderlyingType(underlyingType);

                    // if there isn't one, re-set
                    if (underlyingType == null) underlyingType = memberObj.PropertyType;
                }

                // if underlying type is object
                if (underlyingType == typeof(object) || underlyingType == typeof(Object))
                {
                    // get the value type
                    underlyingType = value.GetType();
                }

                // convert the value to the underlying type
                value = Convert.ChangeType(value, underlyingType);
            }

            // return value
            return value;
        }

        public Type GetMemberType(object o, string member)
        {
            // get member type
            var memberType = o.GetType();

            // get the property
            var memberObj = memberType.GetProperty(member, BindingFlags.Public | BindingFlags.Instance);

            // get property type
            return memberObj.PropertyType;
        }

        public T GetInstanceObject<T>(object obj, string name, Type toCreate, object[] instanceParams)
        {
            // get the type of the parameter
            var ptype = GetMemberType(obj, name);

            //add the model type for the type def of the repository
            Type[] args = { ptype };

            //get the generic type definition for the model
            var constructedField = toCreate.MakeGenericType(args);

            //create instance of the repository, typed for the model
            var instanceField = Activator.CreateInstance(constructedField, instanceParams);

            // return instance
            return (T)instanceField;
        }

        public object GetInstanceObject(Type toCreate, Type typeParam, object[] instanceParams)
        {
            // create arguments array
            Type[] args = { typeParam };

            // create generic type
            var constructedField = toCreate.MakeGenericType(args);

            // get instance
            var instance = Activator.CreateInstance(constructedField, instanceParams);

            // return instance
            return instance;
        }

        public List<string> GetMemberNames(dynamic toUse)
        {
            // create list
            var tList = new List<string>();

            // convert to metadata provider
            var tTarget = toUse as IDynamicMetaObjectProvider;

            // get member names from provider
            if (tTarget != null)
                tList.AddRange(tTarget.GetMetaObject(Expression.Constant(tTarget)).GetDynamicMemberNames());
            // get from normal type
            else
                tList.AddRange(GetMemberNames((Type)toUse.GetType()));

            // return list of members
            return tList;
        }

        public List<string> GetMemberNames(Type toUse)
        {
            var fieldNames = new List<string>();

            // get object type and properties
            var objType = toUse;
            var properties = objType.GetProperties();

            //igore any properties which have ignore set 
            properties = (from p in properties
                          select p).ToArray();

            // iternate through properties
            foreach (var p in properties)
            {
                // get field name
                var name = p.Name;

                // add to set
                fieldNames.Add(name);
            }

            return fieldNames;
        }

        public bool IsNullableType(Type toCheck)
        {
            // determine kind of type
            bool genericType = toCheck.IsGenericType && toCheck.GetGenericTypeDefinition() == typeof(Nullable<>);
            bool primitiveOrValueType = toCheck.IsPrimitive || toCheck.IsValueType;

            // if it is a generic type, it is nullable, if it is primitive, it isnt
            return genericType || !primitiveOrValueType;
        }

        public bool IsNullableType<T>()
        {
            // determine if nullable
            return IsNullableType(typeof(T));
        }

        public Type GetUnderlyingType<T>(T someType)
        {
            return GetUnderlyingType<T>();
        }

        public Type GetUnderlyingType(Type t)
        {
            // check if type is nullable
            var isNullable = IsNullableType(t);

            // default to provided type
            var underlyingType = t;

            // if type is nullable
            if (isNullable)
            {
                // get underlying type value
                underlyingType = Nullable.GetUnderlyingType(underlyingType);
            }

            // if no underlying type, return provided
            return underlyingType ?? t;
        }

        public Type GetUnderlyingType<T>()
        {
            // get underlying type
            return GetUnderlyingType(typeof(T));
        }

        public bool DetermineIfAnonymous(Type type)
        {
            // check for properties of anon types
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");

            // check if both are true
            var isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            // return if anon
            return isAnonymousType;
        }
    }
}
