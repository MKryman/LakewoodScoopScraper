import React, { useState, useEffect } from 'react';
import { ChatLeft } from 'react-bootstrap-icons';
import axios from 'axios';

const App = () => {

    const [items, setItems] = useState([]);

    useEffect(() => {

        const loadData = async () => {
            const { data } = await axios.get('/api/scraper/scrape');
            setItems(data);
        }

        loadData();
    }, []);

    return (
        <div className='container mt-5'>
            <div className='row mt-3'>
                <table className='table table-bordered'>
                    <thead>
                        <tr>
                            <th>Image</th>
                            <th>Title</th>
                            <th>Text</th>
                        </tr>
                    </thead>
                    <tbody>
                        {items.map(item => {
                            return <tr key={item.url}>
                                <td>
                                    <img src={item.image} style={{ width: 200 }} />
                                </td>
                                <td style={{ width: 750 }}>
                                    <a target='_blank' href={item.url}>{item.title}</a>
                                    <br />
                                    <p style={{ fontSize: 20, marginBottom: -5 }}>Posted on: {item.datePosted}</p>
                                    <ChatLeft></ChatLeft><a target='_blank' href={item.commentsLink} className='btn' style={{marginLeft:2, fontSize: 16 }}>{item.comments} Comments</a>
                                </td>
                                <td>
                                    {item.textBlurb}
                                </td>
                            </tr>
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    )
}
export default App;